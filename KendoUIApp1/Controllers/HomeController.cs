using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

namespace KendoUIApp1.Controllers
{
    public class HomeController : Controller
    {
        private String rootPath = "DHSFloodApexProgramPrototype";
        private String pathSeperator = "/";

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.inlineDefault = PopulateTreeView(); // GetDefaultInlineData();
            //ViewBag.inline = GetInlineData();

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            var userPrinciple = User as ClaimsPrincipal;

            return View(userPrinciple);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*
         private IEnumerable<DataItem> GetInlineData()
         {
             List<DataItem> inline = new List<DataItem>
                 {
                     new DataItem
                     {
                         DataName = "Storage",
                         DataUrl = "",
                         DataType = "FOLDER",
                         DataIcon = "~/Content/folder.png",

                         SubData = new List<DataItem>
                         {
                             new DataItem()
                             {
                                 DataName = "Wall Shelving",
                                 DataIcon = "~/Content/wave.png",
                                 DataType = "APP"
                             },
                             new DataItem
                             {
                                  DataName = "Floor Shelving",
                                 DataIcon = "~/Content/wave.png",
                                 DataType = "GIS"

                            },
                             new DataItem
                             {
                                  DataName = "Kids Storag",
                                 DataIcon = "~/Content/wave.png",
                                 DataType = "DOC"
                            }
                         }
                     },
                     new DataItem
                     {
                         DataName = "Lights",
                         DataUrl = "",
                         DataType = "FOLDER",
                         DataIcon = "~/Content/folder.png",
                        SubData = new List<DataItem>
                         {
                             new DataItem()
                             {
                                 DataName = "Ceiling",
                                 DataIcon = "~/Content/wave.png"
                            },
                             new DataItem
                             {
                                  DataName = "Table",
                                 DataIcon = "~/Content/wave.png"
                             },
                             new DataItem
                             {
                                  DataName = "Floor",
                                 DataIcon = "~/Content/wave.png"
                             }
                         }
                     }
                 };

             return inline;
         }
         */

        private IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> PopulateTreeView()
        {
            // Contact Zookeeper
            ZKUtility zkUtil = new ZKUtility();

            List<Kendo.Mvc.UI.TreeViewItemModel> inlineDefault = new List<Kendo.Mvc.UI.TreeViewItemModel>();

            // Get top level items
            List<String> topLevelStr = null;
            while (topLevelStr == null)
            {
                topLevelStr = zkUtil.GetChildren(pathSeperator + rootPath);
            }

            // Lets build a tree
            foreach (String focusArea in topLevelStr)
            {
                Kendo.Mvc.UI.TreeViewItemModel focusModel = new Kendo.Mvc.UI.TreeViewItemModel();
                focusModel.Items = new List<Kendo.Mvc.UI.TreeViewItemModel>();

                // Set zookeeper path for next level
                String path = pathSeperator + rootPath + pathSeperator + focusArea;

                // Set the node in the tree
                focusModel.ImageUrl = "~/Content/folder.png";
                focusModel.Text = focusArea.Substring(1);

                // ask zookeeper for the list
                List<String> femaZones = zkUtil.GetChildren(path);

                foreach (String femaZone in femaZones)
                {
                    Kendo.Mvc.UI.TreeViewItemModel femaModel = new Kendo.Mvc.UI.TreeViewItemModel();
                    femaModel.Items = new List<Kendo.Mvc.UI.TreeViewItemModel>();

                    femaModel.ImageUrl = "~/Content/folder.png";
                    femaModel.Text = femaZone.Substring(1);

                    List<Kendo.Mvc.UI.TreeViewItemModel> zoneChildren = new List<Kendo.Mvc.UI.TreeViewItemModel>();

                    // Look for children
                    String femaPath = path + pathSeperator + femaZone;
                    List<String> dataList = zkUtil.GetChildren(femaPath);

                    foreach (String dataItem in dataList)
                    {
                        // Get data details
                        String localPath = femaPath + pathSeperator + dataItem;
                        String type = zkUtil.GetChildren(localPath + pathSeperator + "TYPE")[0];

                        // Create model entry
                        Kendo.Mvc.UI.TreeViewItemModel dataModel = new Kendo.Mvc.UI.TreeViewItemModel();
                        dataModel.Text = dataItem.Substring(1);
                        dataModel.Url = Uri.UnescapeDataString(zkUtil.GetChildren(localPath + pathSeperator + "URL")[0]);

                        switch (type)
                        {
                            case "GIS":
                                {
                                    dataModel.ImageUrl = "~/Content/gis.gif";
                                }
                                break;
                            case "APP":
                                {
                                    dataModel.ImageUrl = "~/Content/app.gif";
                                }
                                break;
                            case "DOC":
                                {
                                    dataModel.ImageUrl = "~/Content/doc.gif";
                                }
                                break;
                        }
                        femaModel.Items.Add(dataModel);
                    }
                    Kendo.Mvc.UI.TreeViewItemModel myStuff = new Kendo.Mvc.UI.TreeViewItemModel();
                    myStuff.Text = "Key Cloak Protected App";
                    myStuff.ImageUrl = "~/Content/app.gif";
                    myStuff.Url = "http://dhsdata.purvisms.com";
                    femaModel.Items.Add(myStuff);

                    focusModel.Items.Add(femaModel);
                }
                inlineDefault.Add(focusModel);
            }

            zkUtil.Close();
            zkUtil = null;
            GC.Collect();

            return inlineDefault;
        }

        /*
        private IEnumerable<Models.TreeViewItemModel> PopulateMyTreeView()
        {
            // Contact Zookeeper
            ZKUtility zkUtil = new ZKUtility();

            List<Models.TreeViewItemModel> inlineDefault = new List<Models.TreeViewItemModel>();

            // Get top level items
            List<String> topLevelStr = zkUtil.GetChildren(pathSeperator + rootPath);

            // Lets build a tree
            foreach (String focusArea in topLevelStr)
            {
                Models.TreeViewItemModel focusModel = new Models.TreeViewItemModel();
                focusModel.Items = new List<Models.TreeViewItemModel>();

                // Set zookeeper path for next level
                String path = pathSeperator + rootPath + pathSeperator + focusArea;

                // Set the node in the tree
                focusModel.ImageUrl = "~/Content/folder.png";
                focusModel.Text = focusArea.Substring(1);

                // ask zookeeper for the list
                List<String> femaZones = zkUtil.GetChildren(path);

                foreach (String femaZone in femaZones)
                {
                    Models.TreeViewItemModel femaModel = new Models.TreeViewItemModel();
                    femaModel.Items = new List<Models.TreeViewItemModel>();

                    femaModel.ImageUrl = "~/Content/folder.png";
                    femaModel.Text = femaZone.Substring(1);

                    List<Models.TreeViewItemModel> zoneChildren = new List<Models.TreeViewItemModel>();

                    // Look for children
                    String femaPath = path + pathSeperator + femaZone;
                    List<String> dataList = zkUtil.GetChildren(femaPath);

                    foreach (String dataItem in dataList)
                    {
                        // Get data details
                        String localPath = femaPath + pathSeperator + dataItem;
                        String type = zkUtil.GetChildren(localPath + pathSeperator + "TYPE")[0];

                        // Create model entry
                        Models.TreeViewItemModel dataModel = new Models.TreeViewItemModel();
                        dataModel.Text = dataItem.Substring(1);
                        dataModel.Url = Uri.UnescapeDataString(zkUtil.GetChildren(localPath + pathSeperator + "URL")[0]);

                        switch (type)
                        {
                            case "GIS":
                                {
                                    dataModel.ImageUrl = "~/Content/gis.gif";
                                }
                                break;
                            case "APP":
                                {
                                    dataModel.ImageUrl = "~/Content/app.gif";
                                }
                                break;
                            case "DOC":
                                {
                                    dataModel.ImageUrl = "~/Content/doc.gif";
                                }
                                break;
                        }
                        femaModel.Items.Add(dataModel);
                    }
                    Models.TreeViewItemModel myStuff = new Models.TreeViewItemModel();
                    myStuff.Text = "Pass through Security";
                    myStuff.Url = "http://dhsdata.purvisms.com";
                    femaModel.Items.Add(myStuff);

                    focusModel.Items.Add(femaModel);
                }
                inlineDefault.Add(focusModel);
            }

            zkUtil.Close();
            zkUtil = null;
            GC.Collect();

            return inlineDefault;
        }
        */
    }
}
