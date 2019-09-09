using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xRssReader.Models;

namespace xRssReader.ViewModels
{
	public class FeedItemCollection
	{
		private readonly string defaultRssUrl;

		private readonly IList<FeedItem> feedItems;

		public string DefaultRssUrl => defaultRssUrl;

		public IList<FeedItem> FeedItems => feedItems;

		public FeedItemCollection(string defaultRSSUrl, IList<FeedItem> feedItems)
		{
			this.defaultRssUrl = defaultRSSUrl;
			this.feedItems = feedItems;
		}
	}
}