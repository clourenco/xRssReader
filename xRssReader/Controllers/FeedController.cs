using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using xRssReader.Models;
using xRssReader.ViewModels;

namespace xRssReader.Controllers
{
	public class FeedController : Controller
	{
		private const string ColumnHeaderTitleIdentifier = "title";

		private const string ColumnHeaderPubDateIdentifier = "pubdate";

		private const string AscendingSortOrderIdentifier = "asc";

		private const string DescendingSortOrderIdentifier = "desc";

		private readonly string defaultRssUrl;

		public FeedController()
		{
			defaultRssUrl = ConfigurationManager.AppSettings["DefaultRssUrl"];
		}

		[HttpGet]
		public ActionResult Index()
		{
			FeedItemCollection viewModel = null;
			IList<FeedItem> feedItems = GetFeed(defaultRssUrl);

			if (feedItems != null && feedItems.Count > 0)
			{
				viewModel = new FeedItemCollection(defaultRssUrl, feedItems);
			}

			ViewBag.TitleSortOrder = AscendingSortOrderIdentifier;
			ViewBag.PubDateSortOrder = AscendingSortOrderIdentifier;

			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Index(string theRssUrl)
		{
			string rssUrl;
			IList<FeedItem> feedItems = null;
			FeedItemCollection viewModel = null;

			if (String.IsNullOrEmpty(theRssUrl))
			{
				rssUrl = defaultRssUrl;
			}
			else
			{
				rssUrl = theRssUrl;
			}

			feedItems = GetFeed(rssUrl);
			viewModel = new FeedItemCollection(rssUrl, feedItems);

			ViewBag.TitleSortOrder = AscendingSortOrderIdentifier;
			ViewBag.PubDateSortOrder = AscendingSortOrderIdentifier;

			return View(viewModel);
		}

		public ActionResult GetItemsSorted(string rssUrl, string sortByField, string sortOrder)
		{
			IList<FeedItem> feedItems = GetFeed(rssUrl);
			FeedItemCollection viewModel = new FeedItemCollection(rssUrl, feedItems);

			if (feedItems != null && feedItems.Count > 0)
			{
				feedItems = SortFeed(sortByField, sortOrder, feedItems);
				viewModel = new FeedItemCollection(rssUrl, feedItems);

				if (sortByField == ColumnHeaderTitleIdentifier)
				{
					if (sortOrder == AscendingSortOrderIdentifier)
					{
						ViewBag.TitleSortOrder = DescendingSortOrderIdentifier;
					}
					else
					{
						ViewBag.TitleSortOrder = AscendingSortOrderIdentifier;
					}
				}
				else if (sortByField == ColumnHeaderPubDateIdentifier)
				{
					if (sortOrder == AscendingSortOrderIdentifier)
					{
						ViewBag.PubDateSortOrder = DescendingSortOrderIdentifier;
					}
					else
					{
						ViewBag.PubDateSortOrder = AscendingSortOrderIdentifier;
					}
				}
			}

			return PartialView("_FeedItems", viewModel);
		}

		private IList<FeedItem> SortFeed(string sortByField, string sortOrder, IList<FeedItem> feedItems)
		{
			if (!String.IsNullOrEmpty(sortByField) && !String.IsNullOrEmpty(sortOrder))
			{
				if (sortByField == ColumnHeaderTitleIdentifier)
				{
					if (sortOrder == AscendingSortOrderIdentifier)
					{
						feedItems = feedItems.OrderBy(item => item.Title).ToList();
					}
					else
					{
						feedItems = feedItems.OrderByDescending(item => item.Title).ToList();
					}
				}
				else if (sortByField == ColumnHeaderPubDateIdentifier)
				{
					if (sortOrder == AscendingSortOrderIdentifier)
					{
						feedItems = feedItems.OrderBy(item => item.PubDate).ToList();
					}
					else
					{
						feedItems = feedItems.OrderByDescending(item => item.PubDate).ToList();
					}
				}
			}

			return feedItems;
		}

		private IList<FeedItem> GetFeed(string url)
		{
			IList<FeedItem> feedItems = null;

			using (XmlReader reader = XmlReader.Create(url))
			{
				SyndicationFeed feed = SyndicationFeed.Load(reader);
				feedItems = feed.Items.Select(
					item => new FeedItem(
						item.Title.Text,
						item.Links.First().Uri.ToString(),
						item.Summary.Text,
						item.PublishDate.DateTime.ToString())).ToList();

			}

			return feedItems;
		}
	}
}