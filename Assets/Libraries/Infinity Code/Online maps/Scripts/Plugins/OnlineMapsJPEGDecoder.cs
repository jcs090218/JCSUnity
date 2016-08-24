/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// Software JPEG decoder.\n
/// Is used when there are problems in iOS.
/// </summary>
public class OnlineMapsJPEGDecoder
{
    private struct Code
    {
        public byte bits;
        public byte code;
    };

    private class Component
    {
        public int cid;
        public int ssx, ssy;
        public int width, height;
        public int stride;
        public int qtsel;
        public int actabsel, dctabsel;
        public int dcpred;
        public byte[] pixels;
    };

    private class Context
    {
        public byte[] posb;
        public JPEGResult error;
        public int pos;
        public int size;
        public int length;
        public int width, height;
        public int mbwidth, mbheight;
        public int mbsizex, mbsizey;
        public int ncomp;
        public readonly Component[] comp;
        public readonly byte[][] qtab;
        public readonly Code[][] vlctab;
        public int buf, bufbits;
        public int rstinterval;
        public byte[] rgb;

        public Context()
        {
            posb = null;
            comp = new Component[3];
            //block = new int[64];
            qtab = new byte[4][];
            vlctab = new Code[4][];
            for (byte i = 0; i < 4; i++)
            {
                qtab[i] = new byte[64];
                vlctab[i] = new Code[65536];
                if (i < comp.Length)
                {
                    comp[i] = new Component();
                }
            }
        }
    }

    private class JPEGException : Exception { }

    private enum JPEGResult
    {
        OK = 0,
        NO_JPEG,
        UNSUPPORTED,
        OUT_OF_MEM,
        INTERNAL_ERR,
        SYNTAX_ERROR,
        FINISHED,
    };

    private const int CF4A = -9;
    private const int CF4B = 111;
    private const int CF4C = 29;
    private const int CF4D = -3;
    private const int CF3A = 28;
    private const int CF3B = 109;
    private const int CF3C = -9;
    private const int CF3X = 104;
    private const int CF3Y = 27;
    private const int CF3Z = -3;
    private const int CF2A = 139;
    private const int CF2B = -11;

    private const int W1 = 2841;
    private const int W2 = 2676;
    private const int W3 = 2408;
    private const int W5 = 1609;
    private const int W6 = 1108;
    private const int W7 = 565;

    private static readonly byte[] ZZ =
    { 0, 1, 8, 16, 9, 2, 3, 10, 17, 24, 32, 25, 18,
        11, 4, 5, 12, 19, 26, 33, 40, 48, 41, 34, 27, 20, 13, 6, 7, 14, 21, 28, 35,
        42, 49, 56, 57, 50, 43, 36, 29, 22, 15, 23, 30, 37, 44, 51, 58, 59, 52, 45,
        38, 31, 39, 46, 53, 60, 61, 54, 47, 55, 62, 63 };

    private Context context;

    private void ByteAlign()
    {
        context.bufbits &= 0xF8;
    }

    private static byte CF(int x)
    {
        int v = ((x) + 64) >> 7;
        return (byte)((v < 0) ? 0 : ((v > 0xff) ? 255 : v));
    }

    private void ColIDCT(int[] blk, int coef, byte[] pixels, int outv, int stride)
    {
        int x0, x1, x2, x3, x4, x5, x6, x7;
        if (((x1 = blk[coef + 8 * 4] << 8)
             | (x2 = blk[coef + 8 * 6])
             | (x3 = blk[coef + 8 * 2])
             | (x4 = blk[coef + 8 * 1])
             | (x5 = blk[coef + 8 * 7])
             | (x6 = blk[coef + 8 * 5])
             | (x7 = blk[coef + 8 * 3])) == 0)
        {
            int v0 = ((blk[coef] + 32) >> 6) + 128;
            x1 = (byte)((v0 < 0) ? 0 : ((v0 > 0xff) ? 255 : v0));
            for (x0 = 8; x0 != 0; --x0)
            {
                pixels[outv] = (byte)x1;
                outv += stride;
            }
            return;
        }
        x0 = (blk[coef] << 8) + 8192;
        int x8 = W7 * (x4 + x5) + 4;
        x4 = (x8 + (W1 - W7) * x4) >> 3;
        x5 = (x8 - (W1 + W7) * x5) >> 3;
        x8 = W3 * (x6 + x7) + 4;
        x6 = (x8 - (W3 - W5) * x6) >> 3;
        x7 = (x8 - (W3 + W5) * x7) >> 3;
        x8 = x0 + x1;
        x0 -= x1;
        x1 = W6 * (x3 + x2) + 4;
        x2 = (x1 - (W2 + W6) * x2) >> 3;
        x3 = (x1 + (W2 - W6) * x3) >> 3;
        x1 = x4 + x6;
        x4 -= x6;
        x6 = x5 + x7;
        x5 -= x7;
        x7 = x8 + x3;
        x8 -= x3;
        x3 = x0 + x2;
        x0 -= x2;
        x2 = (181 * (x4 + x5) + 128) >> 8;
        x4 = (181 * (x4 - x5) + 128) >> 8;

        int v1 = ((x7 + x1) >> 14) + 128;
        int v2 = ((x3 + x2) >> 14) + 128;
        int v3 = ((x0 + x4) >> 14) + 128;
        int v4 = ((x8 + x6) >> 14) + 128;
        int v5 = ((x8 - x6) >> 14) + 128;
        int v6 = ((x0 - x4) >> 14) + 128;
        int v7 = ((x3 - x2) >> 14) + 128;
        int v8 = ((x7 - x1) >> 14) + 128;

        pixels[outv] = (byte)((v1 < 0) ? 0 : ((v1 > 0xff) ? 255 : v1)); outv += stride;
        pixels[outv] = (byte)((v2 < 0) ? 0 : ((v2 > 0xff) ? 255 : v2)); outv += stride;
        pixels[outv] = (byte)((v3 < 0) ? 0 : ((v3 > 0xff) ? 255 : v3)); outv += stride;
        pixels[outv] = (byte)((v4 < 0) ? 0 : ((v4 > 0xff) ? 255 : v4)); outv += stride;
        pixels[outv] = (byte)((v5 < 0) ? 0 : ((v5 > 0xff) ? 255 : v5)); outv += stride;
        pixels[outv] = (byte)((v6 < 0) ? 0 : ((v6 > 0xff) ? 255 : v6)); outv += stride;
        pixels[outv] = (byte)((v7 < 0) ? 0 : ((v7 > 0xff) ? 255 : v7)); outv += stride;
        pixels[outv] = (byte)((v8 < 0) ? 0 : ((v8 > 0xff) ? 255 : v8));
    }

    private void Convert()
    {
        for (int i = 0; i < context.ncomp; ++i)
        {
            Component c = context.comp[i];
            while ((c.width < context.width) || (c.height < context.height))
            {
                if (c.width < context.width) UpsampleH(c);
                if (context.error != JPEGResult.OK) return;
                if (c.height < context.height) UpsampleV(c);
                if (context.error != JPEGResult.OK) return;
            }
            if ((c.width < context.width) || (c.height < context.height)) Throw(JPEGResult.INTERNAL_ERR);
        }
        if (context.ncomp == 3)
        {
            int prgb = 0, py = 0, pcb = 0, pcr = 0;
            for (int yy = context.height; yy != 0; --yy)
            {
                for (int x = 0; x < context.width; ++x)
                {
                    int y = (context.comp[0].pixels[py + x] << 8) + 128;
                    int cb = context.comp[1].pixels[pcb + x] - 128;
                    int cr = context.comp[2].pixels[pcr + x] - 128;
                    int r = (y + 359 * cr) >> 8;
                    int g = (y - 88 * cb - 183 * cr) >> 8;
                    int b = (y + 454 * cb) >> 8;
                    context.rgb[prgb++] = (byte)((r < 0) ? 0 : ((r > 0xff) ? 255 : r));
                    context.rgb[prgb++] = (byte)((g < 0) ? 0 : ((g > 0xff) ? 255 : g));
                    context.rgb[prgb++] = (byte)((b < 0) ? 0 : ((b > 0xff) ? 255 : b));
                }
                py += context.comp[0].stride;
                pcb += context.comp[1].stride;
                pcr += context.comp[2].stride;
            }
        }
        else if (context.comp[0].width != context.comp[0].stride)
        {
            int pin = context.comp[0].stride;
            int pout = context.comp[0].width;
            int y;
            for (y = context.comp[0].height - 1; y != 0; --y)
            {
                Buffer.BlockCopy(context.comp[0].pixels,
                    pout,
                    context.comp[0].pixels,
                    pin,
                    context.comp[0].width);
                pin += context.comp[0].stride;
                pout += context.comp[0].width;
            }
            context.comp[0].stride = context.comp[0].width;
        }
    }

    private void Decode(byte[] jpeg)
    {
        try
        {
            context = new Context {posb = jpeg, pos = 0, size = jpeg.Length & 0x7FFFFFFF};
            if (context.size < 2) Throw(JPEGResult.NO_JPEG);
            if (((context.posb[context.pos] ^ 0xFF) | (context.posb[context.pos + 1] ^ 0xD8)) != 0) Throw(JPEGResult.NO_JPEG);
            Skip(2);
            while (context.error == JPEGResult.OK)
            {
                //if ((context.size < 2) || (context.posb[context.pos] != 0xFF)) Throw(JPEGResult.SYNTAX_ERROR);
                Skip(2);
                switch (context.posb[context.pos - 1])
                {
                    case 0xC0: DecodeSOF(); break;
                    case 0xC4: DecodeDHT(); break;
                    case 0xDB: DecodeDQT(); break;
                    case 0xDD: DecodeDRI(); break;
                    case 0xDA: DecodeScan(); break;
                    case 0xFE: SkipMarker(); break;
                    default:
                        if ((context.posb[context.pos - 1] & 0xF0) == 0xE0)
                            SkipMarker();
                        else
                            Throw(JPEGResult.UNSUPPORTED);
                        break;
                }
            }
            if (context.error != JPEGResult.FINISHED) return;
            context.error = JPEGResult.OK;
            Convert();
        }
        catch (JPEGException)
        {}
    }

    private ushort Decode16(int pos)
    {
        return (ushort)((context.posb[pos] << 8) | context.posb[pos + 1]);
    }

    private void DecodeBlock(Component c, int outv)
    {
        byte discard = 0;
        byte code = 0;
        int coef = 0;
        int[] block = new int[64];
        c.dcpred += GetVLC(context.vlctab[c.dctabsel], ref discard);
        block[0] = (c.dcpred) * context.qtab[c.qtsel][0];
        do
        {
            int value = GetVLC(context.vlctab[c.actabsel], ref code);
            if (code == 0) break; // EOB
            if ((code & 0x0F) == 0 && (code != 0xF0)) Throw(JPEGResult.SYNTAX_ERROR);
            coef += (code >> 4) + 1;
            if (coef > 63) Throw(JPEGResult.SYNTAX_ERROR);
            block[ZZ[coef]] = value * context.qtab[c.qtsel][coef];
        } while (coef < 63);
        for (coef = 0; coef < 64; coef += 8)
            RowIDCT(block, coef);
        for (coef = 0; coef < 8; ++coef)
            ColIDCT(block, coef, c.pixels, outv + coef, c.stride);
    }

    private void DecodeDHT()
    {
        DecodeLength();
        while (context.length >= 17)
        {
            int i = context.posb[context.pos];
            if ((i & 0xEC) != 0) Throw(JPEGResult.SYNTAX_ERROR);
            if ((i & 0x02) != 0) Throw(JPEGResult.UNSUPPORTED);
            i = (i | (i >> 3)) & 3;
            int cpos = context.pos;
            Skip(17);
            Code[] vlc = context.vlctab[i];
            int vlcc = 0;
            int spread;
            int remain = spread = 65536;
            for (int codelen = 1; codelen <= 16; ++codelen)
            {
                spread >>= 1;
                int currcnt = context.posb[cpos + codelen];
                if (currcnt == 0) continue;
                if (context.length < currcnt) Throw(JPEGResult.SYNTAX_ERROR);
                remain -= currcnt << (16 - codelen);
                if (remain < 0) Throw(JPEGResult.SYNTAX_ERROR);
                for (i = 0; i < currcnt; ++i)
                {
                    byte code = context.posb[context.pos + i];
                    for (int j = spread; j != 0; --j)
                    {
                        if (vlcc < 65536)
                        {
                            vlc[vlcc].bits = (byte)codelen;
                            vlc[vlcc].code = code;
                            vlcc++;
                        }
                    }
                }
                Skip(currcnt);
            }
            while (remain-- != 0)
            {
                if (vlcc < 65536)
                {
                    vlc[vlcc].bits = 0;
                    vlcc++;
                }
            }
        }
        if (context.length != 0) Throw(JPEGResult.SYNTAX_ERROR);
    }

    private void DecodeDQT()
    {
        DecodeLength();
        while (context.length >= 65)
        {
            int i = context.posb[context.pos];
            if ((i & 0xFC) != 0) Throw(JPEGResult.SYNTAX_ERROR);
            byte[] t = context.qtab[i];
            for (i = 0; i < 64; ++i)
                t[i] = context.posb[context.pos + i + 1];
            Skip(65);
        }
        if (context.length != 0) Throw(JPEGResult.SYNTAX_ERROR);
    }

    private void DecodeDRI()
    {
        DecodeLength();
        if (context.length < 2) Throw(JPEGResult.SYNTAX_ERROR);
        context.rstinterval = Decode16(context.pos);
        Skip(context.length);
    }

    private void DecodeLength()
    {
        if (context.size < 2) Throw(JPEGResult.SYNTAX_ERROR);
        context.length = Decode16(context.pos);
        if (context.length > context.size) Throw(JPEGResult.SYNTAX_ERROR);
        Skip(2);
    }

    private void DecodeScan()
    {
        int i, mbx, mby;
        int rstcount = context.rstinterval, nextrst = 0;
        Component c;
        DecodeLength();
        if (context.length < (4 + 2 * context.ncomp)) Throw(JPEGResult.SYNTAX_ERROR);
        if (context.posb[context.pos] != context.ncomp) Throw(JPEGResult.UNSUPPORTED);
        Skip(1);
        for (i = 0; i < context.ncomp; ++i)
        {
            c = context.comp[i];
            if (context.posb[context.pos] != c.cid) Throw(JPEGResult.SYNTAX_ERROR);
            if ((context.posb[context.pos + 1] & 0xEE) != 0) Throw(JPEGResult.SYNTAX_ERROR);
            c.dctabsel = context.posb[context.pos + 1] >> 4;
            c.actabsel = (context.posb[context.pos + 1] & 1) | 2;
            Skip(2);
        }
        if (context.posb[context.pos] != 0 || (context.posb[context.pos + 1] != 63) || context.posb[context.pos + 2] != 0) Throw(JPEGResult.UNSUPPORTED);
        Skip(context.length);
        for (mbx = mby = 0; ; )
        {
            for (i = 0; i < context.ncomp; ++i)
            {
                c = context.comp[i];
                for (int sby = 0; sby < c.ssy; ++sby)
                {
                    for (int sbx = 0; sbx < c.ssx; ++sbx)
                    {
                        DecodeBlock(c, ((mby * c.ssy + sby) * c.stride + mbx * c.ssx + sbx) << 3);
                        if (context.error != JPEGResult.OK) Throw(context.error);
                    }
                }
            }
            if (++mbx >= context.mbwidth)
            {
                mbx = 0;
                if (++mby >= context.mbheight) break;
            }
            if (context.rstinterval != 0 && (--rstcount) != 0)
            {
                ByteAlign();
                i = GetBits(16);
                if (((i & 0xFFF8) != 0xFFD0) || ((i & 7) != nextrst)) Throw(JPEGResult.SYNTAX_ERROR);
                nextrst = (nextrst + 1) & 7;
                rstcount = context.rstinterval;
                for (i = 0; i < 3; ++i)
                    context.comp[i].dcpred = 0;
            }
        }
        context.error = JPEGResult.FINISHED;
    }

    private void DecodeSOF()
    {
        int i, ssxmax = 0, ssymax = 0;
        Component c;
        DecodeLength();
        if (context.length < 9) Throw(JPEGResult.SYNTAX_ERROR);
        if (context.posb[context.pos] != 8) Throw(JPEGResult.UNSUPPORTED);
        context.height = Decode16(context.pos + 1);
        context.width = Decode16(context.pos + 3);
        context.ncomp = context.posb[context.pos + 5];
        Skip(6);
        switch (context.ncomp)
        {
            case 1:
            case 3:
                break;
            default:
                Throw(JPEGResult.UNSUPPORTED);
                break;
        }
        if (context.length < (context.ncomp * 3)) Throw(JPEGResult.SYNTAX_ERROR);
        for (i = 0; i < context.ncomp; ++i)
        {
            c = context.comp[i];
            c.cid = context.posb[context.pos];
            if ((c.ssx = context.posb[context.pos + 1] >> 4) == 0) Throw(JPEGResult.SYNTAX_ERROR);
            if ((c.ssx & (c.ssx - 1)) != 0) Throw(JPEGResult.UNSUPPORTED); // non-power of two
            if ((c.ssy = context.posb[context.pos + 1] & 15) == 0) Throw(JPEGResult.SYNTAX_ERROR);
            if ((c.ssy & (c.ssy - 1)) != 0) Throw(JPEGResult.UNSUPPORTED); // non-power of two
            if (((c.qtsel = context.posb[context.pos + 2]) & 0xFC) != 0) Throw(JPEGResult.SYNTAX_ERROR);
            Skip(3);
            if (c.ssx > ssxmax) ssxmax = c.ssx;
            if (c.ssy > ssymax) ssymax = c.ssy;
        }
        if (context.ncomp == 1)
        {
            c = context.comp[0];
            c.ssx = c.ssy = ssxmax = ssymax = 1;
        }
        context.mbsizex = ssxmax << 3;
        context.mbsizey = ssymax << 3;
        context.mbwidth = (context.width + context.mbsizex - 1) / context.mbsizex;
        context.mbheight = (context.height + context.mbsizey - 1) / context.mbsizey;
        for (i = 0; i < context.ncomp; ++i)
        {
            c = context.comp[i];
            c.width = (context.width * c.ssx + ssxmax - 1) / ssxmax;
            //c.stride = (c.width + 7) & 0x7FFFFFF8;
            c.height = (context.height * c.ssy + ssymax - 1) / ssymax;
            c.stride = context.mbwidth * context.mbsizex * c.ssx / ssxmax;
            if (((c.width < 3) && (c.ssx != ssxmax)) || ((c.height < 3) && (c.ssy != ssymax))) Throw(JPEGResult.UNSUPPORTED);
            c.pixels = new byte[c.stride * (context.mbheight * context.mbsizey * c.ssy / ssymax)];
        }
        if (context.ncomp == 3)
        {
            context.rgb = new byte[context.width * context.height * context.ncomp];
            if (context.rgb == null) Throw(JPEGResult.OUT_OF_MEM);
        }
        Skip(context.length);
    }

    private int GetBits(int bits)
    {
        int res = ShowBits(bits);
        SkipBits(bits);
        return res;
    }

    /// <summary>
    /// Loads JPEG and returns the array colors of the image.
    /// </summary>
    /// <param name="bytes">JPEG file data.</param>
    /// <returns>Array of colors.</returns>
    public static Color32[] GetColors(byte[] bytes)
    {
        OnlineMapsJPEGDecoder jpeg = new OnlineMapsJPEGDecoder();
        jpeg.Decode(bytes);

        Color32[] colors = new Color32[jpeg.context.rgb.Length / 3];
        int w = jpeg.context.width;
        int h = jpeg.context.height;

        for (int i = 0; i < colors.Length; i++)
        {
            int i3 = i * 3;
            int cx = i % w;
            int cy = h - i / w - 1;

            colors[cx + cy * w] = new Color32(jpeg.context.rgb[i3], jpeg.context.rgb[i3 + 1], jpeg.context.rgb[i3 + 2], 255);
        }

        return colors;
    }

    private int GetVLC(Code[] vlc, ref byte code)
    {
        int value = ShowBits(16);
        int bits = vlc[value].bits;
        if (bits == 0)
        {
            context.error = JPEGResult.SYNTAX_ERROR; 
            return 0;
        }
        SkipBits(bits);
        value = vlc[value].code;
        code = (byte)value;
        bits = value & 15;
        if (bits == 0) return 0;
        value = ShowBits(bits);
        SkipBits(bits);
        if (value < (1 << (bits - 1))) value += ((-1) << bits) + 1;
        return value;
    }

    private void RowIDCT(int[] blk, int coef)
    {
        int x1, x2, x3, x4, x5, x6, x7;
        if (((x1 = blk[coef + 4] << 11)
        | (x2 = blk[coef + 6])
        | (x3 = blk[coef + 2])
        | (x4 = blk[coef + 1])
        | (x5 = blk[coef + 7])
        | (x6 = blk[coef + 5])
        | (x7 = blk[coef + 3])) == 0)
        {
            blk[coef] = blk[coef + 1] = blk[coef + 2] = blk[coef + 3] = blk[coef + 4] = blk[coef + 5] = blk[coef + 6] = blk[coef + 7] = blk[coef] << 3;
            return;
        }
        int x0 = (blk[coef] << 11) + 128;
        int x8 = W7 * (x4 + x5);
        x4 = x8 + (W1 - W7) * x4;
        x5 = x8 - (W1 + W7) * x5;
        x8 = W3 * (x6 + x7);
        x6 = x8 - (W3 - W5) * x6;
        x7 = x8 - (W3 + W5) * x7;
        x8 = x0 + x1;
        x0 -= x1;
        x1 = W6 * (x3 + x2);
        x2 = x1 - (W2 + W6) * x2;
        x3 = x1 + (W2 - W6) * x3;
        x1 = x4 + x6;
        x4 -= x6;
        x6 = x5 + x7;
        x5 -= x7;
        x7 = x8 + x3;
        x8 -= x3;
        x3 = x0 + x2;
        x0 -= x2;
        x2 = (181 * (x4 + x5) + 128) >> 8;
        x4 = (181 * (x4 - x5) + 128) >> 8;
        blk[coef] = (x7 + x1) >> 8;
        blk[coef + 1] = (x3 + x2) >> 8;
        blk[coef + 2] = (x0 + x4) >> 8;
        blk[coef + 3] = (x8 + x6) >> 8;
        blk[coef + 4] = (x8 - x6) >> 8;
        blk[coef + 5] = (x0 - x4) >> 8;
        blk[coef + 6] = (x3 - x2) >> 8;
        blk[coef + 7] = (x7 - x1) >> 8;
    }

    private int ShowBits(int bits)
    {
        if (bits == 0) return 0;
        while (context.bufbits < bits)
        {
            if (context.size <= 0)
            {
                context.buf = (context.buf << 8) | 0xFF;
                context.bufbits += 8;
                continue;
            }
            byte newbyte = context.posb[context.pos++];
            context.size--;
            context.bufbits += 8;
            context.buf = (context.buf << 8) | newbyte;
            if (newbyte == 0xFF)
            {
                if (context.size != 0)
                {
                    byte marker = context.posb[context.pos++];
                    context.size--;
                    switch (marker)
                    {
                        case 0x00:
                        case 0xFF:
                            break;
                        case 0xD9: context.size = 0; break;
                        default:
                            if ((marker & 0xF8) != 0xD0)
                                context.error = JPEGResult.SYNTAX_ERROR;
                            else
                            {
                                context.buf = (context.buf << 8) | marker;
                                context.bufbits += 8;
                            }
                            break;
                    }
                }
                else
                    context.error = JPEGResult.SYNTAX_ERROR;
            }
        }
        return (context.buf >> (context.bufbits - bits)) & ((1 << bits) - 1);
    }

    private void Skip(int count)
    {
        context.pos += count;
        context.size -= count;
        context.length -= count;
        if (context.size < 0) context.error = JPEGResult.SYNTAX_ERROR;
    }

    private void SkipBits(int bits)
    {
        if (context.bufbits < bits)
            ShowBits(bits);
        context.bufbits -= bits;
    }

    private void SkipMarker()
    {
        DecodeLength();
        Skip(context.length);
    }

    private void Throw(JPEGResult e)
    {
        context.error = e;
        throw new JPEGException();
    }


    private void UpsampleH(Component c)
    {
        int xmax = c.width - 3;
        int lin = 0, lout = 0;
        byte[] outv = new byte[(c.width * c.height) << 1];
        for (int y = c.height; y != 0; --y)
        {
            int linp1 = lin + 1;
            int linp2 = lin + 2;
            int linp3 = lin + 3;
            outv[lout] = CF(CF2A * c.pixels[lin] + CF2B * c.pixels[linp1]);
            outv[lout + 1] = CF(CF3X * c.pixels[lin] + CF3Y * c.pixels[linp1] + CF3Z * c.pixels[linp2]);
            outv[lout + 2] = CF(CF3A * c.pixels[lin] + CF3B * c.pixels[linp1] + CF3C * c.pixels[linp2]);

            int loutp3 = lout + 3;
            int loutp4 = lout + 4;

            for (int x = 0; x < xmax; ++x)
            {
                outv[loutp3 + (x << 1)] = CF(CF4A * c.pixels[lin + x] + CF4B * c.pixels[linp1 + x] + CF4C * c.pixels[linp2 + x] + CF4D * c.pixels[linp3 + x]);
                outv[loutp4 + (x << 1)] = CF(CF4D * c.pixels[lin + x] + CF4C * c.pixels[linp1 + x] + CF4B * c.pixels[linp2 + x] + CF4A * c.pixels[linp3 + x]);
            }
            lin += c.stride;
            lout += c.width << 1;
            int lins1 = lin - 1;
            int lins2 = lin - 2;
            int lins3 = lin - 3;
            outv[lout - 3] = CF(CF3A * c.pixels[lins1] + CF3B * c.pixels[lins2] + CF3C * c.pixels[lins3]);
            outv[lout - 2] = CF(CF3X * c.pixels[lins1] + CF3Y * c.pixels[lins2] + CF3Z * c.pixels[lins3]);
            outv[lout - 1] = CF(CF2A * c.pixels[lins1] + CF2B * c.pixels[lins2]);
        }
        c.width <<= 1;
        c.stride = c.width;
        c.pixels = outv;
    }

    private void UpsampleV(Component c)
    {
        int w = c.width, s1 = c.stride, s2 = s1 + s1;
        byte[] outv = new byte[(c.width * c.height) << 1];
        for (int x = 0; x < w; ++x)
        {
            int cin = x;
            int cout = x;
            outv[cout] = CF(CF2A * c.pixels[cin] + CF2B * c.pixels[cin + s1]); cout += w;
            outv[cout] = CF(CF3X * c.pixels[cin] + CF3Y * c.pixels[cin + s1] + CF3Z * c.pixels[cin + s2]); cout += w;
            outv[cout] = CF(CF3A * c.pixels[cin] + CF3B * c.pixels[cin + s1] + CF3C * c.pixels[cin + s2]); cout += w;
            cin += s1;
            for (int y = c.height - 3; y != 0; --y)
            {
                outv[cout] = CF(CF4A * c.pixels[cin + -s1] + CF4B * c.pixels[cin] + CF4C * c.pixels[cin + s1] + CF4D * c.pixels[cin + s2]); cout += w;
                outv[cout] = CF(CF4D * c.pixels[cin + -s1] + CF4C * c.pixels[cin] + CF4B * c.pixels[cin + s1] + CF4A * c.pixels[cin + s2]); cout += w;
                cin += s1;
            }
            cin += s1;
            outv[cout] = CF(CF3A * c.pixels[cin] + CF3B * c.pixels[cin - s1] + CF3C * c.pixels[cin - s2]); cout += w;
            outv[cout] = CF(CF3X * c.pixels[cin] + CF3Y * c.pixels[cin - s1] + CF3Z * c.pixels[cin - s2]); cout += w;
            outv[cout] = CF(CF2A * c.pixels[cin] + CF2B * c.pixels[cin - s1]);
        }
        c.height <<= 1;
        c.stride = c.width;
        c.pixels = outv;
    }
}