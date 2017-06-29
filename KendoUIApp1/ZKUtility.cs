using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace KendoUIApp1
{
    public class ZKUtility : ZKUtil
    {

        private ZooKeeper zk;

        public ZKUtility(string zkURL)
        {
            zk = CreateClient();
        }

        public ZKUtility()
        {
            zk = CreateClient();
        }

        public void Close()
        {
            zk.Dispose();
            zk = null;

            GC.Collect();
        }

        public List<String> GetChildren(String path)
        {

            var p = zk.GetChildren(path, false, null);
            List<String> retList = p.OrderBy(e => e).ToList();


            return retList;
        }

        public void Publish2ZK()
        {
            string name = "/DHSFloodApexProgramPrototype";
            zk.Create(name, name.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);





            string dataName = name + "/aPreparedness";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateFemaRegions(dataName);

            dataName = name + "/aResponse";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateFemaRegions(dataName);

            dataName = name + "/aRecovery";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateFemaRegions(dataName);


            /*
            //https://earthobs1.arcgis.com/arcgis/rest/services/Live_Stream_Gauges/MapServer
            var p = zk.GetChildren("/", false, null);
            List<string> c_a = children_s;
            var c_b = p;
            c_a = c_a.OrderBy(e => e).ToList();
            c_b = c_b.OrderBy(e => e).ToList();
            */
        }

        private void CreateSampleData(String dataName)
        {
            List<string> children = new List<string>();
            List<string> children_s = new List<string>();

            children.Add(dataName + "/NOAA Stream Gauge Data");
            children.Add(children[0] + "/" + Uri.EscapeDataString("https://earthobs1.arcgis.com/arcgis/rest/services/Live_Stream_Gauges/MapServer"));

            children.Add(dataName + "/NDFD 72 Hour Precipitation");
            children.Add(children[2] + "/" + Uri.EscapeDataString("https://livefeeds.arcgis.com/arcgis/rest/services/LiveFeeds/NDFD_Precipitation/MapServer"));

            children.Add(dataName + "/nowCOAST Current Precipitation");
            children.Add(children[4] + "/" + "https%3A%2F%2Fnowcoast.noaa.gov%2Farcgis%2Frest%2Fservices%2Fnowcoast%2Fradar_meteo_imagery_nexrad_time%2FMapServer");

            children.Add(dataName + "/ESRI Hydro Reference");
            children.Add(children[6] + "/" + Uri.EscapeDataString("https://tiles.arcgis.com/tiles/P3ePLMYs2RVChkJx/arcgis/rest/services/Esri_Hydro_Reference_Overlay/MapServer?cacheKey=b6101388776b08b9"));

            for (int i = 0; i < children.Count; i++)
            {
                string childname = children[i];
                zk.Create(childname, childname.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }

        }
        private void CreateFemaRegions(String name)
        {
            string dataName = name + "/FEMA Region I";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region II";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region III";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region IV";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region V";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region VI";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region VII";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region VIII";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region IX";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);

            dataName = name + "/FEMA Region X";
            zk.Create(dataName, dataName.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            CreateSampleData(dataName);
        }
    }
}
