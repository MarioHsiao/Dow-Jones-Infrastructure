﻿using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Ajax;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using EntityLinkData = DowJones.Ajax.Article.EntityLinkData;

namespace DowJones.MvcShowcase.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            var articleDataSet = new ArticleResultset
            {
                AccessionNo = "J000000020120628e86s0002m",
                PictureSize = PictureSize.Large,
                ContentCategory = ContentCategory.Publication,
                ContentCategoryDescriptor = ContentCategory.Publication.ToString(),
                ContentSubCategory = ContentSubCategory.Article,
                ContentSubCategoryDescriptor = ContentSubCategory.Article.ToString(),
                OriginalContentCategory = ContentSubCategory.Article.ToString(),
                ExternalUri = "",
                Head = new List<RenderItem>
                {
                    new RenderItem
                        {
                        ItemText = "J Logo",
                        ItemValue = "http://global.factiva.com/FactivaLogos/jLogo.gif",
                        ItemMarkUp = MarkUpType.HeadLogo
                    }
                },
                Source = new List<RenderItem>
                {
                    new RenderItem
                    {
                        ItemMarkUp = MarkUpType.EntityLink,
                        ItemEntityData = new EntityLinkData
                        {
                            Code = "J",
                            Name = "The Wall Street Journal",
                            Category = "source"
                        }
                    }
                },
                SourceCode = "J",
                SourceName = "The Wall Street Journal",
                ByLine = new List<RenderItem>
                {
                    new RenderItem
                    {
                        ItemText = "Mike Vilensky ",
                        ItemMarkUp = MarkUpType.Plain
                    }
                },
                Authors = new List<RenderItem>
                {
                    new RenderItem
                    {
                        ItemMarkUp = MarkUpType.EntityLink,
                        ItemEntityData = new EntityLinkData
                        {
                            Code = "3433960",
                            Name = "Mike Vilensky",
                            Category = "author"
                        }
                    }
                },
                PublicationDate = "28 June 2012",
                PublicationTime = "12:00 AM",
                ModificationDate = "28 June 2012",
                ModificationTime = "2:20 AM",
                Headline = new List<RenderItem>
                {
                    new RenderItem
                    {
                        ItemText = "Heard & Scene: Actor at Home on High",
                        ItemMarkUp = MarkUpType.Plain
                    }
                },
                Copyright = new List<RenderItem>
                {
                    new RenderItem
                    {
                        ItemText = "(Copyright (c) 2012, Dow Jones & Company, Inc.) ",
                        ItemMarkUp = MarkUpType.Plain
                    }
                },
                LeadParagraph = new List<Dictionary<string, List<RenderItem>>>
                {
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "If the movie star Andrew Garfield weren't promoting his anticipated, big-budget action movie, \"The Amazing Spider-Man,\" he would be climbing Mount Kilimanjaro.",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "Naturally.",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                },
                TailParagraphs = new List<Dictionary<string, List<RenderItem>>>
                {
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "\"I wanted to [climb it] this year but this press tour came up!\" said Mr. Garfield, in the penthouse of the Royalton Hotel Tuesday evening. \"I like heights. I find heights quite fun and invigorating.\"",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "The actor is an experienced rock-climber, he explained.",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "So why not forgo the presstour altogether? Skip elevator rides to hotel rooms in favor of scaling the heights of Africa?",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "\"I wish,\" Mr. Garfield said, laughing. \"I could send in body doubles and clones.\" Alas, he said, a reporter would be able to tell the difference.",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "In addition to an aspiring Kilimanjaro conqueror, Mr. Garfield is now the \"ambassador of sport\" for Worldwide Orphans, a nonprofit run by Dr. Jane Aronson that seeks to improve the lives of orphaned children. (Spider-Man, in the famed comic books, was an orphan, after all, Dr. Aronson pointed out.)",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    },
                    new Dictionary<string, List<RenderItem>>
                    {
                        {
                            "display",
                            new List<RenderItem>
                            {
                                new RenderItem
                                {
                                    ItemText = "The actor seems to be good at an inordinate number of things. \"I used to be a gymnast, a swimmer, a football player, a rugby player and a cricket player,\" he recalled. \"It doesn't matter whether you win or lose, it's how you play the game -- but that's something I didn't learn growing up. I thought if I lost then I was nothing, which is absolutely wrong, I have realized.\"",
                                    ItemMarkUp = MarkUpType.Plain
                                }
                            }
                        }
                    }
                },
                Language = "English",
                LanguageCode = "en",
                WordCount = 458,
                Pages = new List<string>
                {
                    "A26"
                },
                IndexingCodeSets = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "in",
                        new Dictionary<string, string>
                        {
                            {"i971", "Motion Pictures/Sound Recording"},
                            {"i97411", "Broadcasting"},
                            {"iconssv", "Consumer Services"},
                            {"imed", "Media"},
                            {"ibcs", "Business/Consumer Services"}
                        }
                    },
                    {
                        "ns",
                        new Dictionary<string, string>
                        {
                            {"gent", "Arts/Entertainment"},
                            {"gcat", "Political/General News"}
                        }
                    },
                    {
                        "re",
                        new Dictionary<string, string>
                        {
                            {"nyc", "New York City"},
                            {"usa", "United States"},
                            {"use", "Northeast U.S."},
                            {"usny", "New York"},
                            {"namz", "North America"}
                        }
                    },
                },
                PublisherName = "Dow Jones & Company, Inc.",
                Ipcs = new List<string>
                {
                    "CYC",
                    "NND",
                    "GNY",
                    "LMJ"
                },
                MimeType = "text/xml"
            };

            var articleModel = new ArticleModel
            {
                ArticleDataSet = articleDataSet,
                ShowPostProcessing = true,
                PostProcessingOptions = new[]
    		    {
    		       	PostProcessingOptions.Print,
    		       	PostProcessingOptions.Save,
    		       	PostProcessingOptions.PressClips,
    		       	PostProcessingOptions.Email, 
    		       	PostProcessingOptions.Listen,
    		       	PostProcessingOptions.Translate,
    		       	PostProcessingOptions.Share
    		    },
                ShowSourceLinks = true
            };
            return View(articleModel);
        }

        [Route("Article/data/{mode}")]
        public ActionResult Data(string mode = "cs")
        {
            var view = mode == "cs" ? "_cSharpData" : "_jsData";

            return PartialView(view);
        }
    }
}