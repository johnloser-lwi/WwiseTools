using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;
using WwiseTools.Audio;

namespace WwiseTools.Utils
{
    public class WwiseUtility
    {
        public static void Init(string project_path, string file_path = @"", bool commitCopy = false) 
        {
            WwiseUtility.project_path = project_path;
            WwiseUtility.file_path = file_path;
            WwiseUtility.commitCopy = commitCopy;
            GetWwiseDefaultConversionSettings();
            GetMasterAudioBus();
        }

        public static WwiseEvent ToEvent(string name, WwiseUnit unit, WwiseWorkUnit workUnit, WwiseAction.ActionType actionType)
        {
            if (String.IsNullOrEmpty(name)) name = unit.name;

            WwiseAction action = ToAction(unit, workUnit, actionType);

            WwiseEvent e = new WwiseEvent(name);
            e.AddChild(action);

            return e;
        }

        public static WwiseAction ToAction(WwiseUnit unit, WwiseWorkUnit workUnit, WwiseAction.ActionType actionType)
        {
            WwiseAction action = new WwiseAction(actionType, new WwiseObjectRef(unit.name, unit.id, workUnit.id));

            return action;
        }

        public static WwiseEvent ToEvent(string name, WwiseAction[] actionList)
        {
            WwiseEvent e = new WwiseEvent(name);
            foreach (var action in actionList)
            {
                e.AddChild(action);
            }

            return e;
        }

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
        }

        private static string file_path;
        private static string project_path;

        private static  WwiseNodeWithName default_conversion_settings;
        private static WwiseNodeWithName master_audio_bus;

        public static string FilePath { get => file_path; set => file_path = value; }
        public static string ProjectPath { get => project_path; }

        public static bool CommitCopy { get => commitCopy; set => commitCopy = value; }

        private static bool commitCopy = false;

        public static WwiseNodeWithName DefualtConversionSettings { get => default_conversion_settings;  }
        public static WwiseNodeWithName MasterAudioBus { get => master_audio_bus; }

        private static void GetWwiseDefaultConversionSettings()
        {
            WwiseParser parser = new WwiseParser();
            string[] file = parser.Parse(@"Conversion Settings\Default Work Unit.wwu");
            WwiseWorkUnit wu = parser.GetWorkUnit(file);
            WwiseUnit unit = parser.GetUnitByName("Default Conversion Settings", file);
            string id = unit.id;
            string workUnitId = wu.id;
            WwiseNodeWithName reference = new WwiseNodeWithName("Reference", "Conversion");
            reference.AddChildNode(new WwiseObjectRef("Default Conversion Settings", id, workUnitId));

            default_conversion_settings = reference;
        }

        private static void GetMasterAudioBus()
        {

            WwiseParser parser = new WwiseParser();
            string[] file = parser.Parse("Master-Mixer Hierarchy\\Default Work Unit.wwu");
            WwiseWorkUnit wu = parser.GetWorkUnit(file);
            WwiseUnit unit = parser.GetUnitByName("Master Audio Bus", file);
            string id = unit.id;
            string workUnitId = wu.id;
            WwiseNodeWithName reference = new WwiseNodeWithName("Reference", "OutputBus");
            reference.AddChildNode(new WwiseObjectRef("Master Audio Bus", id, workUnitId));

            master_audio_bus = reference;
        }
        
    }
}
