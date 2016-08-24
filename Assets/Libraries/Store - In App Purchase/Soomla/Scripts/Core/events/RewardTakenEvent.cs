using System;
using System.Collections;

namespace Soomla
{
	public class RewardTakenEvent : SoomlaEvent
	{
		public readonly Reward Reward;

		public RewardTakenEvent (String rewardId) : this(rewardId, null)
		{

		}

		public RewardTakenEvent (Reward reward) : this(reward, null)
		{

		}

		public RewardTakenEvent (String rewardId, Object sender) : base(sender)
		{
			this.Reward = Reward.GetReward (rewardId);
		}

		public RewardTakenEvent (Reward reward, Object sender) : base(sender)
		{
			this.Reward = reward;
		}
	}
}