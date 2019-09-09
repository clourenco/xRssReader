using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xRssReader.Models
{
	public class FeedItem
	{
		//Title
		//Link
		//Description
		//PubDate

		private readonly string title;

		private readonly string link;

		private readonly string description;

		private readonly string pubDate;

		public string Title => title;

		public string Link => link;

		public string Description => description;

		public string PubDate => pubDate;


		public FeedItem(string title, string link, string description, string pubDate)
		{
			this.title = title;
			this.link = link;
			this.description = description;
			this.pubDate = pubDate;
		}
	}
}