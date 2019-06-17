using System;
using System.IO;
using Foundation;

namespace Datamodel
{
    public class ShareLibs
    {
        public static string GetDatabasePath()
        {
            var FileManager = new NSFileManager();
            var appGroupContainer = FileManager.GetContainerUrl("group.pro.ozzies.moviebookmarker");
            var appGroupContainerPath = appGroupContainer.Path;
            return Path.Combine(appGroupContainerPath, "ormdemo.db3");
        }

        public ShareLibs()
        {

        }
    }
}
