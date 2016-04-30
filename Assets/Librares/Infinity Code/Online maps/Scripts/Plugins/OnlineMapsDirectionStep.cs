/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Step of the route found by the OnlineMapsFindDirection.
/// </summary>
public class OnlineMapsDirectionStep
{
    /// <summary>
    /// The total distance covered by this step.
    /// </summary>
    public int distance;

    /// <summary>
    /// The total duration of the passage of this step.
    /// </summary>
    public int duration;

    /// <summary>
    /// Location of the endpoint of this step (lat, lng). 
    /// </summary>
    public Vector2 end;

    /// <summary>
    /// HTML instructions to the current step.
    /// </summary>
    public string instructions;

    /// <summary>
    /// Maneuver the current step.
    /// </summary>
    public string maneuver;

    /// <summary>
    /// A list of locations of points included in the current step.
    /// </summary>
    public List<Vector2> points;

    /// <summary>
    /// Location of the startpoint of this step (lat, lng). 
    /// </summary>
    public Vector2 start;

    /// <summary>
    /// Instructions to the current step without HTML tags.
    /// </summary>
    public string stringInstructions;

    private OnlineMapsDirectionStep()
    {
        
    }

    /// <summary>
    /// Constructor. \n
    /// <strong>Please do not use. </strong>\n
    /// Use OnlineMapsDirectionStep.TryParse.
    /// </summary>
    /// <param name="node">XMLNode of route</param>
    private OnlineMapsDirectionStep(OnlineMapsXML node)
    {
        start = node.GetLatLng("start_location");
        end = node.GetLatLng("end_location");
        duration = node.Find<int>("duration/value");
        instructions = node.Find<string>("html_instructions");
        GetStringInstructions();
        distance = node.Find<int>("distance/value");

        maneuver = node.Find<string>("maneuver");
        
        string encodedPoints = node.Find<string>("polyline/points");
        points = OnlineMapsGoogleAPIQuery.DecodePolylinePoints(encodedPoints);
    }

    /// <summary>
    /// Converts a list of the steps of the route to list of point locations.
    /// </summary>
    /// <param name="steps">List of the steps of the route.</param>
    /// <returns>A list of locations of route.</returns>
    public static List<Vector2> GetPoints(List<OnlineMapsDirectionStep> steps)
    {
        List<Vector2> routePoints = new List<Vector2>();

        foreach (OnlineMapsDirectionStep step in steps)
        {
            if (routePoints.Count > 0) routePoints.RemoveAt(routePoints.Count - 1);
            routePoints.AddRange(step.points);
        }

        return routePoints;
    }

    private void GetStringInstructions()
    {
        if (string.IsNullOrEmpty(instructions)) return;
        stringInstructions = StrReplace(instructions, 
            new[] {"&lt;", "&gt;", "&nbsp;", "&amp;", "&amp;nbsp;"},
            new[] {"<",    ">",    " ",      "&",     " "});
        stringInstructions = Regex.Replace(stringInstructions, "<div.*?>", "\n");
        stringInstructions = Regex.Replace(stringInstructions, "<.*?>", string.Empty);
    }

    private string StrReplace(string str, string[] origin, string[] replace)
    {
        if (origin == null || replace == null) return str;

        for (int i = 0; i < Mathf.Min(origin.Length, replace.Length); i++) str = str.Replace(origin[i], replace[i]);
        return str;
    }

    /// <summary>
    /// Converts the route obtained by OnlineMapsFindDirection, a list of the steps of the route.
    /// </summary>
    /// <param name="route">Route obtained by OnlineMapsFindDirection.</param>
    /// <returns>List of OnlineMapsDirectionStep or null.</returns>
    public static List<OnlineMapsDirectionStep> TryParse(string route)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(route);

            OnlineMapsXML direction = xml.Find("//DirectionsResponse");
            if (direction.isNull) return null;

            string status = direction.Find<string>("status");
            if (status != "OK") return null;

            OnlineMapsXMLList legNodes = direction.FindAll("route/leg");
            if (legNodes == null || legNodes.count == 0) return null;

            List<OnlineMapsDirectionStep> steps = new List<OnlineMapsDirectionStep>();

            foreach (OnlineMapsXML legNode in legNodes)
            {
                OnlineMapsXMLList stepNodes = legNode.FindAll("step");
                if (stepNodes.count == 0) continue;

                foreach (OnlineMapsXML step in stepNodes)
                {
                    OnlineMapsDirectionStep navigationStep = new OnlineMapsDirectionStep(step);
                    steps.Add(navigationStep);
                }
            }

            return steps;
        }
        catch { }

        return null;
    }

    /// <summary>
    /// Converts the route obtained by OnlineMapsOpenRouteService, a list of the steps of the route.
    /// </summary>
    /// <param name="route">Route obtained by OnlineMapsOpenRouteService.</param>
    /// <returns>List of OnlineMapsDirectionStep or null.</returns>
    public static List<OnlineMapsDirectionStep> TryParseORS(string route)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(route);
            OnlineMapsXMLNamespaceManager nsmgr = xml.GetNamespaceManager();
            OnlineMapsXML errorNode = xml.Find("//xls:ErrorList/xls:Error", nsmgr);
            if (!errorNode.isNull) return null;

            OnlineMapsXMLList instructionNodes = xml.FindAll("//xls:RouteInstruction", nsmgr);
            List<OnlineMapsDirectionStep> steps = new List<OnlineMapsDirectionStep>();

            foreach (OnlineMapsXML node in instructionNodes)
            {
                OnlineMapsDirectionStep step = new OnlineMapsDirectionStep();
                step.points = new List<Vector2>();

                OnlineMapsXML geometry = node.Find("xls:RouteInstructionGeometry", nsmgr);
                OnlineMapsXML line = geometry[0];
                foreach (OnlineMapsXML pointNode in line)
                {
                    string coordsStr = pointNode.Value();
                    string[] coords = coordsStr.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    Vector2 coordsV = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
                    step.points.Add(coordsV);
                }

                step.distance = (int)(node.Find("xls:Distance", nsmgr).A<float>("value") * 1000);
                step.instructions = node.Find("xls:Instruction", nsmgr).Value();

                steps.Add(step);
            }

            return steps;
        }
        catch (Exception exception)
        {
            Debug.Log("Exception: " + exception.Message + "\n" + exception.StackTrace);
        }
        return null;
    }

    /// <summary>
    /// Converts the route obtained by OnlineMapsFindDirection, to array of list of the steps of the route.
    /// </summary>
    /// <param name="route">Route obtained by OnlineMapsFindDirection.</param>
    /// <returns>Array of list of OnlineMapsDirectionStep or null.</returns>
    public static List<OnlineMapsDirectionStep>[] TryParseWithAlternatives(string route)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(route);

            OnlineMapsXML direction = xml.Find("//DirectionsResponse");
            if (direction.isNull) return null;

            string status = direction.Get<string>("status");
            if (status != "OK") return null;

            OnlineMapsXMLList routes = direction.FindAll("route");
            List<OnlineMapsDirectionStep>[] result = new List<OnlineMapsDirectionStep>[routes.count];

            for(int i = 0; i < routes.count; i++)
            {
                OnlineMapsXMLList legNodes = routes[i].FindAll("leg");
                if (legNodes == null || legNodes.count == 0) continue;

                List<OnlineMapsDirectionStep> steps = new List<OnlineMapsDirectionStep>();

                foreach (OnlineMapsXML legNode in legNodes)
                {
                    OnlineMapsXMLList stepNodes = legNode.FindAll("step");
                    if (stepNodes.count == 0) continue;

                    foreach (OnlineMapsXML step in stepNodes)
                    {
                        OnlineMapsDirectionStep navigationStep = new OnlineMapsDirectionStep(step);
                        steps.Add(navigationStep);
                    }
                }

                result[i] = steps;
            }

            return result;
        }
        catch { }

        return null;
    }
}