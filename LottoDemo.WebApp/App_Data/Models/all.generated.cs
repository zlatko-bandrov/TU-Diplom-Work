using  System;
using  System.Collections.Generic;
using  System.Linq.Expressions;
using  System.Web;
using  Umbraco.Core.Models;
using  Umbraco.Core.Models.PublishedContent;
using  Umbraco.Web;
using  Umbraco.ModelsBuilder;
using  Umbraco.ModelsBuilder.Umbraco;
[assembly: PureLiveAssembly]
[assembly:ModelsBuilderAssembly(PureLive = true, SourceHash = "65d1b388c8b0c960")]
[assembly:System.Reflection.AssemblyVersion("0.0.0.1")]


// FILE: models.generated.cs

//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.5.96
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------















namespace Umbraco.Web.PublishedContentModels
{
	/// <summary>HomePage</summary>
	[PublishedContentModel("homePage")]
	public partial class HomePage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "homePage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public HomePage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<HomePage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>Settings</summary>
	[PublishedContentModel("settings")]
	public partial class Settings : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "settings";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Settings(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Settings, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Advertises Root Node
		///</summary>
		[ImplementPropertyType("advertises")]
		public string Advertises
		{
			get { return this.GetPropertyValue<string>("advertises"); }
		}

		///<summary>
		/// Home Latest News Count
		///</summary>
		[ImplementPropertyType("homeLatestNewsCount")]
		public int HomeLatestNewsCount
		{
			get { return this.GetPropertyValue<int>("homeLatestNewsCount"); }
		}

		///<summary>
		/// Lottery List Page
		///</summary>
		[ImplementPropertyType("lotteryListPage")]
		public string LotteryListPage
		{
			get { return this.GetPropertyValue<string>("lotteryListPage"); }
		}

		///<summary>
		/// News List Root
		///</summary>
		[ImplementPropertyType("newsListRoot")]
		public object NewsListRoot
		{
			get { return this.GetPropertyValue("newsListRoot"); }
		}
	}

	/// <summary>Footer</summary>
	[PublishedContentModel("footer")]
	public partial class Footer : Settings
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "footer";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Footer(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Footer, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Footer Text: Enter the footer text
		///</summary>
		[ImplementPropertyType("footerText")]
		public IHtmlString FooterText
		{
			get { return this.GetPropertyValue<IHtmlString>("footerText"); }
		}
	}

	/// <summary>Payment Methods</summary>
	[PublishedContentModel("paymentMethods")]
	public partial class PaymentMethods : Settings
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "paymentMethods";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public PaymentMethods(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<PaymentMethods, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}
	}

	/// <summary>Awards</summary>
	[PublishedContentModel("awards")]
	public partial class Awards : Settings
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "awards";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Awards(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Awards, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}
	}

	/// <summary>Award</summary>
	[PublishedContentModel("awardItem")]
	public partial class AwardItem : Awards
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "awardItem";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public AwardItem(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<AwardItem, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Link
		///</summary>
		[ImplementPropertyType("link")]
		public Newtonsoft.Json.Linq.JArray Link
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JArray>("link"); }
		}

		///<summary>
		/// Link Image
		///</summary>
		[ImplementPropertyType("linkImage")]
		public string LinkImage
		{
			get { return this.GetPropertyValue<string>("linkImage"); }
		}
	}

	// Mixin content Type 1076 with alias "basePage"
	/// <summary>BasePage</summary>
	public partial interface IBasePage : IPublishedContent
	{
		/// <summary>BrowserTitle</summary>
		string BrowserTitle { get; }

		/// <summary>NavigationTitle</summary>
		string NavigationTitle { get; }

		/// <summary>ShowInNavigation</summary>
		bool ShowInMainNavigation { get; }
	}

	/// <summary>BasePage</summary>
	[PublishedContentModel("basePage")]
	public partial class BasePage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "basePage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public BasePage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<BasePage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return GetBrowserTitle(this); }
		}

		/// <summary>Static getter for BrowserTitle</summary>
		public static string GetBrowserTitle(IBasePage that) { return that.GetPropertyValue<string>("browserTitle"); }

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return GetNavigationTitle(this); }
		}

		/// <summary>Static getter for NavigationTitle</summary>
		public static string GetNavigationTitle(IBasePage that) { return that.GetPropertyValue<string>("navigationTitle"); }

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return GetShowInMainNavigation(this); }
		}

		/// <summary>Static getter for ShowInNavigation</summary>
		public static bool GetShowInMainNavigation(IBasePage that) { return that.GetPropertyValue<bool>("showInMainNavigation"); }
	}

	/// <summary>PlayNowPage</summary>
	[PublishedContentModel("playNowPage")]
	public partial class PlayNowPage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "playNowPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public PlayNowPage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<PlayNowPage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>HowToPlay</summary>
	[PublishedContentModel("howToPlay")]
	public partial class HowToPlay : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "howToPlay";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public HowToPlay(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<HowToPlay, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Page Description
		///</summary>
		[ImplementPropertyType("pageDescription")]
		public IHtmlString PageDescription
		{
			get { return this.GetPropertyValue<IHtmlString>("pageDescription"); }
		}

		///<summary>
		/// Page Title
		///</summary>
		[ImplementPropertyType("pageTitle")]
		public string PageTitle
		{
			get { return this.GetPropertyValue<string>("pageTitle"); }
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>ResultsAndWinningsPage</summary>
	[PublishedContentModel("resultsAndWinningsPage")]
	public partial class ResultsAndWinningsPage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "resultsAndWinningsPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public ResultsAndWinningsPage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<ResultsAndWinningsPage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>News List</summary>
	[PublishedContentModel("newsList")]
	public partial class NewsList : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "newsList";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public NewsList(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<NewsList, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Page Sub Title
		///</summary>
		[ImplementPropertyType("pageSubTitle")]
		public string PageSubTitle
		{
			get { return this.GetPropertyValue<string>("pageSubTitle"); }
		}

		///<summary>
		/// Page Title
		///</summary>
		[ImplementPropertyType("pageTitle")]
		public string PageTitle
		{
			get { return this.GetPropertyValue<string>("pageTitle"); }
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>News Article</summary>
	[PublishedContentModel("newsItem")]
	public partial class NewsItem : NewsList
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "newsItem";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public NewsItem(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<NewsItem, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Content Text
		///</summary>
		[ImplementPropertyType("contentText")]
		public IHtmlString ContentText
		{
			get { return this.GetPropertyValue<IHtmlString>("contentText"); }
		}

		///<summary>
		/// Short Description
		///</summary>
		[ImplementPropertyType("shortDescription")]
		public string ShortDescription
		{
			get { return this.GetPropertyValue<string>("shortDescription"); }
		}

		///<summary>
		/// Teaser Image
		///</summary>
		[ImplementPropertyType("teaserImage")]
		public string TeaserImage
		{
			get { return this.GetPropertyValue<string>("teaserImage"); }
		}
	}

	/// <summary>FAQPage</summary>
	[PublishedContentModel("fAQPage")]
	public partial class FAqpage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "fAQPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public FAqpage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<FAqpage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Description
		///</summary>
		[ImplementPropertyType("description")]
		public string Description
		{
			get { return this.GetPropertyValue<string>("description"); }
		}

		///<summary>
		/// Page Title
		///</summary>
		[ImplementPropertyType("pageTitle")]
		public string PageTitle
		{
			get { return this.GetPropertyValue<string>("pageTitle"); }
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>ContactUsPage</summary>
	[PublishedContentModel("contactUsPage")]
	public partial class ContactUsPage : PublishedContentModel, IBasePage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "contactUsPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public ContactUsPage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<ContactUsPage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// BrowserTitle
		///</summary>
		[ImplementPropertyType("browserTitle")]
		public string BrowserTitle
		{
			get { return BasePage.GetBrowserTitle(this); }
		}

		///<summary>
		/// NavigationTitle
		///</summary>
		[ImplementPropertyType("navigationTitle")]
		public string NavigationTitle
		{
			get { return BasePage.GetNavigationTitle(this); }
		}

		///<summary>
		/// ShowInNavigation
		///</summary>
		[ImplementPropertyType("showInMainNavigation")]
		public bool ShowInMainNavigation
		{
			get { return BasePage.GetShowInMainNavigation(this); }
		}
	}

	/// <summary>Lotteries List Page</summary>
	[PublishedContentModel("lotteriesListPage")]
	public partial class LotteriesListPage : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "lotteriesListPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public LotteriesListPage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<LotteriesListPage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}
	}

	/// <summary>Lottery Game</summary>
	[PublishedContentModel("lotteryGame")]
	public partial class LotteryGame : LotteriesListPage
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "lotteryGame";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public LotteryGame(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<LotteryGame, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Bonus Ball Maximum
		///</summary>
		[ImplementPropertyType("bonusBallMaximum")]
		public int BonusBallMaximum
		{
			get { return this.GetPropertyValue<int>("bonusBallMaximum"); }
		}

		///<summary>
		/// Bonus Ball Minimum
		///</summary>
		[ImplementPropertyType("bonusBallMinimum")]
		public int BonusBallMinimum
		{
			get { return this.GetPropertyValue<int>("bonusBallMinimum"); }
		}

		///<summary>
		/// Bonus Balls Count
		///</summary>
		[ImplementPropertyType("bonusBallsCount")]
		public int BonusBallsCount
		{
			get { return this.GetPropertyValue<int>("bonusBallsCount"); }
		}

		///<summary>
		/// Draw Balls Count
		///</summary>
		[ImplementPropertyType("drawBallsCount")]
		public int DrawBallsCount
		{
			get { return this.GetPropertyValue<int>("drawBallsCount"); }
		}

		///<summary>
		/// Drawing Time Interval: During which time the draw is to take place (in minutes)
		///</summary>
		[ImplementPropertyType("drawingTimeInterval")]
		public int DrawingTimeInterval
		{
			get { return this.GetPropertyValue<int>("drawingTimeInterval"); }
		}

		///<summary>
		/// Draw Numbers Maximum
		///</summary>
		[ImplementPropertyType("drawNumbersMaximum")]
		public int DrawNumbersMaximum
		{
			get { return this.GetPropertyValue<int>("drawNumbersMaximum"); }
		}

		///<summary>
		/// Game Description
		///</summary>
		[ImplementPropertyType("gameDescription")]
		public IHtmlString GameDescription
		{
			get { return this.GetPropertyValue<IHtmlString>("gameDescription"); }
		}

		///<summary>
		/// Lottery Logo
		///</summary>
		[ImplementPropertyType("lotteryLogo")]
		public string LotteryLogo
		{
			get { return this.GetPropertyValue<string>("lotteryLogo"); }
		}

		///<summary>
		/// Lottery Name
		///</summary>
		[ImplementPropertyType("lotteryName")]
		public string LotteryName
		{
			get { return this.GetPropertyValue<string>("lotteryName"); }
		}

		///<summary>
		/// Lottery Page Title
		///</summary>
		[ImplementPropertyType("lotteryPageTitle")]
		public string LotteryPageTitle
		{
			get { return this.GetPropertyValue<string>("lotteryPageTitle"); }
		}

		///<summary>
		/// Ticket Price
		///</summary>
		[ImplementPropertyType("ticketPrice")]
		public string TicketPrice
		{
			get { return this.GetPropertyValue<string>("ticketPrice"); }
		}
	}

	/// <summary>How To Play Section</summary>
	[PublishedContentModel("contentSection")]
	public partial class ContentSection : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "contentSection";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public ContentSection(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<ContentSection, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Align Image To Right
		///</summary>
		[ImplementPropertyType("alignImageToRight")]
		public bool AlignImageToRight
		{
			get { return this.GetPropertyValue<bool>("alignImageToRight"); }
		}

		///<summary>
		/// Content Text
		///</summary>
		[ImplementPropertyType("contentText")]
		public IHtmlString ContentText
		{
			get { return this.GetPropertyValue<IHtmlString>("contentText"); }
		}

		///<summary>
		/// Image
		///</summary>
		[ImplementPropertyType("image")]
		public string Image
		{
			get { return this.GetPropertyValue<string>("image"); }
		}

		///<summary>
		/// Section Title
		///</summary>
		[ImplementPropertyType("sectionTitle")]
		public string SectionTitle
		{
			get { return this.GetPropertyValue<string>("sectionTitle"); }
		}
	}

	/// <summary>FAQ Item</summary>
	[PublishedContentModel("fAQItem")]
	public partial class FAqitem : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "fAQItem";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public FAqitem(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<FAqitem, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Answer
		///</summary>
		[ImplementPropertyType("answer")]
		public IHtmlString Answer
		{
			get { return this.GetPropertyValue<IHtmlString>("answer"); }
		}

		///<summary>
		/// Question
		///</summary>
		[ImplementPropertyType("question")]
		public string Question
		{
			get { return this.GetPropertyValue<string>("question"); }
		}
	}

	/// <summary>Right Column Advertise</summary>
	[PublishedContentModel("rightColumnAdvertising")]
	public partial class RightColumnAdvertising : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "rightColumnAdvertising";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public RightColumnAdvertising(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<RightColumnAdvertising, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Image
		///</summary>
		[ImplementPropertyType("image")]
		public string Image
		{
			get { return this.GetPropertyValue<string>("image"); }
		}
	}

	/// <summary>Advertise List</summary>
	[PublishedContentModel("advertiseList")]
	public partial class AdvertiseList : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "advertiseList";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public AdvertiseList(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<AdvertiseList, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}
	}

	/// <summary>Folder</summary>
	[PublishedContentModel("Folder")]
	public partial class Folder : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Folder";
		public new const PublishedItemType ModelItemType = PublishedItemType.Media;
#pragma warning restore 0109

		public Folder(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Folder, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Contents:
		///</summary>
		[ImplementPropertyType("contents")]
		public object Contents
		{
			get { return this.GetPropertyValue("contents"); }
		}
	}

	/// <summary>Image</summary>
	[PublishedContentModel("Image")]
	public partial class Image : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Image";
		public new const PublishedItemType ModelItemType = PublishedItemType.Media;
#pragma warning restore 0109

		public Image(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Image, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Size
		///</summary>
		[ImplementPropertyType("umbracoBytes")]
		public string UmbracoBytes
		{
			get { return this.GetPropertyValue<string>("umbracoBytes"); }
		}

		///<summary>
		/// Type
		///</summary>
		[ImplementPropertyType("umbracoExtension")]
		public string UmbracoExtension
		{
			get { return this.GetPropertyValue<string>("umbracoExtension"); }
		}

		///<summary>
		/// Upload image
		///</summary>
		[ImplementPropertyType("umbracoFile")]
		public Umbraco.Web.Models.ImageCropDataSet UmbracoFile
		{
			get { return this.GetPropertyValue<Umbraco.Web.Models.ImageCropDataSet>("umbracoFile"); }
		}

		///<summary>
		/// Height
		///</summary>
		[ImplementPropertyType("umbracoHeight")]
		public string UmbracoHeight
		{
			get { return this.GetPropertyValue<string>("umbracoHeight"); }
		}

		///<summary>
		/// Width
		///</summary>
		[ImplementPropertyType("umbracoWidth")]
		public string UmbracoWidth
		{
			get { return this.GetPropertyValue<string>("umbracoWidth"); }
		}
	}

	/// <summary>File</summary>
	[PublishedContentModel("File")]
	public partial class File : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "File";
		public new const PublishedItemType ModelItemType = PublishedItemType.Media;
#pragma warning restore 0109

		public File(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<File, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Size
		///</summary>
		[ImplementPropertyType("umbracoBytes")]
		public string UmbracoBytes
		{
			get { return this.GetPropertyValue<string>("umbracoBytes"); }
		}

		///<summary>
		/// Type
		///</summary>
		[ImplementPropertyType("umbracoExtension")]
		public string UmbracoExtension
		{
			get { return this.GetPropertyValue<string>("umbracoExtension"); }
		}

		///<summary>
		/// Upload file
		///</summary>
		[ImplementPropertyType("umbracoFile")]
		public object UmbracoFile
		{
			get { return this.GetPropertyValue("umbracoFile"); }
		}
	}

	/// <summary>Lotto Gamer</summary>
	[PublishedContentModel("lottoGamer")]
	public partial class LottoGamer : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "lottoGamer";
		public new const PublishedItemType ModelItemType = PublishedItemType.Member;
#pragma warning restore 0109

		public LottoGamer(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<LottoGamer, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Country
		///</summary>
		[ImplementPropertyType("country")]
		public string Country
		{
			get { return this.GetPropertyValue<string>("country"); }
		}

		///<summary>
		/// Date Of Birth
		///</summary>
		[ImplementPropertyType("dateOfBirth")]
		public DateTime DateOfBirth
		{
			get { return this.GetPropertyValue<DateTime>("dateOfBirth"); }
		}

		///<summary>
		/// First Name
		///</summary>
		[ImplementPropertyType("firstName")]
		public string FirstName
		{
			get { return this.GetPropertyValue<string>("firstName"); }
		}

		///<summary>
		/// Last Name
		///</summary>
		[ImplementPropertyType("lastName")]
		public string LastName
		{
			get { return this.GetPropertyValue<string>("lastName"); }
		}

		///<summary>
		/// Mobile Phone
		///</summary>
		[ImplementPropertyType("mobilePhone")]
		public string MobilePhone
		{
			get { return this.GetPropertyValue<string>("mobilePhone"); }
		}

		///<summary>
		/// Person Title
		///</summary>
		[ImplementPropertyType("personTitle")]
		public object PersonTitle
		{
			get { return this.GetPropertyValue("personTitle"); }
		}

		///<summary>
		/// Is Approved
		///</summary>
		[ImplementPropertyType("umbracoMemberApproved")]
		public bool UmbracoMemberApproved
		{
			get { return this.GetPropertyValue<bool>("umbracoMemberApproved"); }
		}

		///<summary>
		/// Comments
		///</summary>
		[ImplementPropertyType("umbracoMemberComments")]
		public string UmbracoMemberComments
		{
			get { return this.GetPropertyValue<string>("umbracoMemberComments"); }
		}

		///<summary>
		/// Failed Password Attempts
		///</summary>
		[ImplementPropertyType("umbracoMemberFailedPasswordAttempts")]
		public string UmbracoMemberFailedPasswordAttempts
		{
			get { return this.GetPropertyValue<string>("umbracoMemberFailedPasswordAttempts"); }
		}

		///<summary>
		/// Last Lockout Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastLockoutDate")]
		public string UmbracoMemberLastLockoutDate
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastLockoutDate"); }
		}

		///<summary>
		/// Last Login Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastLogin")]
		public string UmbracoMemberLastLogin
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastLogin"); }
		}

		///<summary>
		/// Last Password Change Date
		///</summary>
		[ImplementPropertyType("umbracoMemberLastPasswordChangeDate")]
		public string UmbracoMemberLastPasswordChangeDate
		{
			get { return this.GetPropertyValue<string>("umbracoMemberLastPasswordChangeDate"); }
		}

		///<summary>
		/// Is Locked Out
		///</summary>
		[ImplementPropertyType("umbracoMemberLockedOut")]
		public bool UmbracoMemberLockedOut
		{
			get { return this.GetPropertyValue<bool>("umbracoMemberLockedOut"); }
		}

		///<summary>
		/// Password Answer
		///</summary>
		[ImplementPropertyType("umbracoMemberPasswordRetrievalAnswer")]
		public string UmbracoMemberPasswordRetrievalAnswer
		{
			get { return this.GetPropertyValue<string>("umbracoMemberPasswordRetrievalAnswer"); }
		}

		///<summary>
		/// Password Question
		///</summary>
		[ImplementPropertyType("umbracoMemberPasswordRetrievalQuestion")]
		public string UmbracoMemberPasswordRetrievalQuestion
		{
			get { return this.GetPropertyValue<string>("umbracoMemberPasswordRetrievalQuestion"); }
		}
	}

}



// EOF
