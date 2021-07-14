using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 默认功能，负责初始化路径以及常用的转换等
    /// </summary>
    public class WwiseUtility
    {
        /// <summary>
        /// 初始化Wwise工程路径、用户指定文件夹路径以及是否自动复制文件
        /// </summary>
        /// <param name="project_path"></param>
        /// <param name="file_path"></param>
        /// <param name="commitCopy"></param>
        public static void Init(string project_path, string file_path = @"", bool commitCopy = false) 
        {
            WwiseUtility.project_path = project_path;
            WwiseUtility.file_path = file_path;
            //WwiseUtility.commitCopy = commitCopy;
            //GetWwiseDefaultConversionSettings();
            //GetMasterAudioBus();
        }


        /// <summary>
        /// 将指定Wwise单元转换为事件，需要指定该单元以及该单元所属的工作单元，并设置Action类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="workUnit"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
       /* public static WwiseEvent ToEvent(string name, WwiseUnit unit, WwiseWorkUnit workUnit, WwiseAction.ActionType actionType)
        {
            if (String.IsNullOrEmpty(name)) name = unit.name;

            WwiseAction action = ToAction(unit, workUnit, actionType);

            WwiseEvent e = new WwiseEvent(name);
            e.AddChild(action);

            return e;
        }

        /// <summary>
        /// 将指定Wwise单元转换成事件中的Action，需要指定该单元以及该单元所属的工作单元，并设置Action类型
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="workUnit"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static WwiseAction ToAction(WwiseUnit unit, WwiseWorkUnit workUnit, WwiseAction.ActionType actionType)
        {
            WwiseAction action = new WwiseAction(actionType, new WwiseObjectRef(unit.name, unit.id, workUnit.id));

            return action;
        }

        /// <summary>
        /// 创建一个事件，并添加一组Action
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionList"></param>
        /// <returns></returns>
        public static WwiseEvent ToEvent(string name, WwiseAction[] actionList)
        {
            WwiseEvent e = new WwiseEvent(name);
            foreach (var action in actionList)
            {
                e.AddChild(action);
            }

            return e;
        }

        /// <summary>
        /// 通过指定文件夹路径生成Music Playlist Container，可设置Playlist Type以及循环次数(默认为1)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="playlistType"></param>
        /// <param name="loopCount"></param>
        /// <returns></returns>
        public static WwiseMusicPlaylistContainer GenerateMusicPlaylistFromFolder(string folderPath, WwiseMusicPlaylistItem.PlaylistType playlistType, int loopCount = 1)
        {
            string path = Path.Combine(file_path, folderPath);
            string[] folders = Directory.GetDirectories(path);
            WwiseMusicPlaylistContainer container = new WwiseMusicPlaylistContainer(folderPath, playlistType, loopCount);
            foreach (var f in folders)
            {
                string folder = new DirectoryInfo(f).Name;
                container.AddSegment(GenerateMusicSegmentFromFolder(Path.Combine(folderPath, folder)));
            }

            return container;
        }

        /// <summary>
        /// 通过指定文件夹路径生成Music Segment，可设置轨道Track Type(默认为Normal)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="trackType"></param>
        /// <returns></returns>
        public static WwiseMusicSegment GenerateMusicSegmentFromFolder(string folderPath, WwiseMusicTrack.TrackType trackType = WwiseMusicTrack.TrackType.Normal)
        {
            string path = Path.Combine(file_path, folderPath);
            string[] fileList = Directory.GetFiles(path);
            WwiseMusicSegment result = new WwiseMusicSegment(new DirectoryInfo(folderPath).Name);

            foreach (var file in fileList)
            {
                string file_name = Path.GetFileName(file);
                string p = Path.Combine(folderPath, file_name);
                if (!file_name.EndsWith(".wav")) continue;
                var track = result.AddTrack(file_name.Replace(".wav", ""), p, trackType);
                track.SetStream(true, false, false);
            }

            return result;
        }

        /// <summary>
        /// 将文件复制到Originals文件夹下
        /// </summary>
        /// <param name="file"></param>
        /// <param name="language"></param>
        public static void CopyFile(string file, string language)
        {

            if (!CommitCopy) return;
            try
            {
                string final_path = Path.Combine(FilePath, file);
                string destination = project_path + @"/Originals" + String.Format(@"/{0}/", language) + file;
                Console.WriteLine(Path.GetDirectoryName(destination));
                if (!Directory.Exists(Path.GetDirectoryName(destination))) Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(final_path, destination, true);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }*/

        private static string file_path;
        private static string project_path;
        private static int schema_version = 97;

        //private static  WwiseNodeWithName default_conversion_settings;
        //private static WwiseNodeWithName master_audio_bus;

        public static string FilePath { get => file_path; set => file_path = value; }
        public static string ProjectPath { get => project_path; }

        public static int SchemaVersion => schema_version;

        /*
        /// <summary>
        /// 设置是否执行复制
        /// </summary>
        public static bool CommitCopy { get => commitCopy; set => commitCopy = value; }

        private static bool commitCopy = false;

        /// <summary>
        /// 获取默认的转码设置(Default Conversion Settings)
        /// </summary>
        public static WwiseNodeWithName DefualtConversionSettings { get => default_conversion_settings;  }

        /// <summary>
        /// 获取默认的总线(Master Audio Bus)
        /// </summary>
        public static WwiseNodeWithName MasterAudioBus { get => master_audio_bus; }
        */
        public static WwiseNodeWithName GetWwiseDefaultConversionSettings(WwiseParser externalParser)
        {
            WwiseParser parser = new WwiseParser();
            parser.Parse(@"Conversion Settings\Default Work Unit.wwu");
            //Console.WriteLine(parser.Document.InnerXml);
            wwiseWorkUnit wu = parser.GetWorkUnit();
            Console.WriteLine(parser.Document.InnerXml);
            wwiseUnit unit = parser.GetUnitByName("Default Conversion Settings", "Conversion");
            string id = unit.ID;
            string workUnitId = wu.ID;
            WwiseNodeWithName reference = new WwiseNodeWithName("Reference", "Conversion", externalParser);
            reference.AddChildNode(new WwiseObjectRef("Default Conversion Settings", id, workUnitId, externalParser));
            return reference;
        }

        public static WwiseNodeWithName GetMasterAudioBus(WwiseParser externalParser)
        {

            WwiseParser parser = new WwiseParser();
            parser.Parse(@"Master-Mixer Hierarchy\Default Work Unit.wwu");
            wwiseWorkUnit wu = parser.GetWorkUnit();
            wwiseUnit unit = parser.GetUnitByName("Master Audio Bus", "Bus");
            string id = unit.ID;
            string workUnitId = wu.ID;
            WwiseNodeWithName reference = new WwiseNodeWithName("Reference", "OutputBus", externalParser);
            reference.AddChildNode(new WwiseObjectRef("Master Audio Bus", id, workUnitId, externalParser));

            //master_audio_bus = reference;

            return reference;
        }
        
    }
}
