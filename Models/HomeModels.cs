using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApi2.Models
{
    public class TranDau
    {
        
        public string ThoiGian { get; set; }
        public string Team1 { get; set; }
        public string TySo { get; set; }
        public string Team2 { get; set; }

    }

    public class BangDau
    {
        public string TenBang { get; set; }
        public List<TranDau> lstTranDau = new List<TranDau>();
    }

    public class HomeModels
    {
        public string GiaiDau { get; set; }

        public List<BangDau> lstLichThiDau = new List<BangDau>();
    }
    public class HomeServices
    {
        public HomeModels ReadContentHtmlPage(HomeModels lichthidauDetail)
        {
            if(lichthidauDetail == null)
            {
                lichthidauDetail = new HomeModels();
            }

            lichthidauDetail.GiaiDau = "Euro 2016";

            string _url = "http://terrikon.com/soccer/euro-2016"; //Website that we need read

            try
            {
                WebProxy proxy = new WebProxy("http://hcm-proxy:9090");
                proxy.Credentials = new NetworkCredential("nhatlm1", "JapaneS1984", "fsoft.fpt.vn");
                proxy.UseDefaultCredentials = true;
                WebRequest.DefaultWebProxy = proxy;
                
                var html = new HtmlDocument();
                WebClient client = new WebClient();
                client.Proxy = proxy;

                string downloadString = client.DownloadString(_url);
                html.LoadHtml(downloadString);
                var root = html.DocumentNode;

                string attribute = "class";
                string attributeName = "maincol";

                var rootGroup = root.Descendants()
                    .Where(n => n.GetAttributeValue(attribute, "").Equals(attributeName))
                    .FirstOrDefault();

                attributeName = "team-info";
                var rootGroupInfor = root.Descendants()
                    .Where(n => n.GetAttributeValue(attribute, "").Equals(attributeName));

                HtmlNodeCollection nodesGroupName = rootGroup.SelectNodes("h2");

                int groupNumber = rootGroupInfor.Count();
                int i = 1;
                TranDau ltd = null;
                BangDau bd = null;

                lichthidauDetail.lstLichThiDau.Clear();

                foreach (HtmlNode node in nodesGroupName)
                {
                    bd = new BangDau();
                    string groupName = node.InnerText;
                    Console.WriteLine(groupName + " \n");
                    bd.TenBang = groupName;

                    HtmlNodeCollection matches = rootGroup.SelectNodes(@"div[" + i + "]/div[2]/table/tr");
                    
                    for (int match = 1; match <= matches.Count; match++)
                    {
                        ltd = new TranDau();
                        string team1 = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[2]").InnerText;

                        string score = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[3]").InnerText;

                        string team2 = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[4]").InnerText;

                        string date = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[6]").InnerText;

                        Console.WriteLine(team1 + " " + score + " " + team2 + " " + date + "  " + " \n\n");

                        
                        ltd.Team1 = team1;
                        ltd.TySo = score;
                        ltd.Team2 = team2;
                        ltd.ThoiGian = date;
                        bd.lstTranDau.Add(ltd);
                    }

                    lichthidauDetail.lstLichThiDau.Add(bd);

                    i++;

                    if (i > groupNumber)
                        break;
                }

            }
            catch (Exception ex)
            {
                //ViewBag.; //báo lỗi không kết nối được trang web làm sao
            }

            return lichthidauDetail;
        }
    }
}