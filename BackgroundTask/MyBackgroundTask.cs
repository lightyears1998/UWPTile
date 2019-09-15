using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class MyBackgroundTask : IBackgroundTask
    {

        /// <summary>
        /// 参考文献：
        /// 1. 微软文档 https://docs.microsoft.com/zh-cn/windows/uwp/launch-resume/update-a-live-tile-from-a-background-task
        /// 2. 云栖社区 https://yq.aliyun.com/articles/259632
        /// </summary>
        /// <param name="taskInstance"></param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 以下代码复制自微软文档并经过魔改

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();  // 如果没有用到异步任务就不需要Defferal
            string saying = GetRandomString();
            UpdateTile(saying);
            deferral.Complete();
        }

        private string GetRandomString()
        {
            string[] sayings =
            {
                "就算是自私……我也希望那些人能够永远都有笑容……",
                "所谓的王，乃最贪婪，最豪爽，最易怒之人。且清且浊，追求极致人生。为臣者，因之而仰慕，追随其左右。因此，臣民才会有称王之志，追寻自己的理想。",
                "人的能力是有极限的。我从短暂的人生当中学到一件事……越是玩弄计谋,就越会发现人类的能力是有极限的……除非超越人类。",
                "邪王真眼是最强的。",
                "舞台上演员不能无视剧本随便演，华丽地退场才是完成使命。",
                "此身为剑所天成，身如钢铁，心似琉璃。",
                "叫做妈妈的人，是不会哭的。",
                "那天，我见过的最强剑客，提起了天下最强的宝剑······却早已没了剑心。",
                "懂得认输是非常重要的，不尝到失败的不甘，也就不会明白跌倒后怎么爬起来，更不可能再向前迈进。",
                "历史虽然会一再重演，但人类却无法回到过去。",
                "翠星石最喜欢苍星石的说,所以到死也要在一起的说...",
                "我不是因为需要你们评论几句才去当英雄的，而是因为我想去当所以我才当的。"
            }; // From https://v1.hitokoto.cn/?c=a

            return sayings[new Random().Next(sayings.Length)];
        }

        private void UpdateTile(string tileText)
        {
            // 以下代码复制自微软文档并经过魔改

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);

            tileXml.GetElementsByTagName("text")[0].InnerText = tileText;

            updater.Update(new TileNotification(tileXml));
        }
    }
}
