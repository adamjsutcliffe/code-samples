using System.IO;
using Peak.Speedoku.Scripts.Common.Localisation;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Editor
{
    public static class PeakPostProcessBuild
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string projectPath)
        {
            Debug.Log("[PostProcessBuild] Start post-process");

            if (target == BuildTarget.Android)
            {
                Debug.Log("[PostProcessBuild] Starting to perform post build tasks for Android platform.");
                RunPostProcessTasksAndroid(projectPath);
            }
            else if (target == BuildTarget.iOS)
            {
                Debug.Log("[PostProcessBuild] Starting to perform post build tasks for Ios platform.");
                RunPostProcessTasksIos(projectPath);
            }

            Debug.Log("[PostProcessBuild] Done post-process");
        }

        private static void RunPostProcessTasksAndroid(string projectPath)
        {
#if UNITY_ANDROID
            //some code here
            Debug.Log($"[PostProcessBuild] Android project path: {projectPath}");
#endif
        }

        private static void RunPostProcessTasksIos(string projectPath)
        {
//#if UNITY_IOS
            Debug.Log("[PostProcessBuild] Starting to perform post build tasks for iOS platform.");

            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);
            
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");

            // 1.
            // We need frameworks to be added to the project:
            // - libxml2.tbd

            //Debug.Log("[PostProcessBuild] Adding libxml2 to Xcode project.");
            //xcodeProject.AddFrameworkToProject(xcodeTarget, "libxml2.tbd", true);
            //Debug.Log("[PostProcessBuild] libxml2 added successfully.");
            

            // 2.
            // The adjust SDK needs to have -lxml2 flag set in other linker flags section because of it's categories.
            // OTHER_LDFLAGS -lxml2

            Debug.Log("[PostProcessBuild] Adding -lxml2 flag to other linker flags (OTHER_LDFLAGS).");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-lxml2");
            Debug.Log("[PostProcessBuild] -lxml2 successfully added to other linker flags.");


            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);


            //MODIFY Info.plist
            // 3. set ad network required usage descriptions for localisation etc

            string plistPath = projectPath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict root = plist.root;

            Debug.Log("[PostProcessBuild] Prepare ad network Info.plist update.");

            const string calenderKey = "NSCalendarsUsageDescription";
            PlistElementString calenderDesc = new PlistElementString("For Ads Calendar events creation.");
            root[calenderKey] = calenderDesc;

            const string photoKey = "NSPhotoLibraryUsageDescription";
            PlistElementString photoDesc = new PlistElementString("For Ads Photos saving.");
            root[photoKey] = photoDesc;

            const string locationKey = "NSLocationAlwaysUsageDescription";
            PlistElementString locationDesc = new PlistElementString("Used for localising ads.");
            root[locationKey] = locationDesc;

            Debug.Log("[PostProcessBuild] Complete ad network Info.plist update.");

            //Localisation setup

            const string localisationKey = "CFBundleLocalizations";
            PlistElementArray localisationsArray = new PlistElementArray();
            //LocalisationSystem.supportedLanguages.Keys;
            foreach (string langKey in LocalisationSystem.supportedLanguages.Keys)
            {
                localisationsArray.AddString(langKey);
            }
            root[localisationKey] = localisationsArray;


            //UPDATE PLIST

            plist.WriteToFile(plistPath);


            //4. Create Fastline files

            Debug.Log("[PostProcessBuild] Create fastfile files if necessary.");

            string pathString = projectPath + "/fastlane";

            if (File.Exists(pathString) == false)
            {
                System.IO.Directory.CreateDirectory(pathString);
                Debug.Log("Create fastlane folder");
            }

            string gemPath = projectPath + "/Gemfile";

            if (File.Exists(gemPath) == false)
            { // do not overwrite
                Debug.Log("Create Gem file");
                using (StreamWriter outfile =
                    new StreamWriter(gemPath))
                {
                    outfile.WriteLine("source 'https://rubygems.org'");
                    outfile.WriteLine("");
                    outfile.WriteLine("gem 'fastlane'");
                    outfile.WriteLine("");
                }

                AssetDatabase.Refresh();
            }


            string fastfilePath = projectPath + "/fastlane/Fastfile";

            if (File.Exists(fastfilePath) == false)
            { // do not overwrite
                Debug.Log("Create fastlane file");
                using (StreamWriter outfile =
                    new StreamWriter(fastfilePath))
                {
                    outfile.WriteLine("# This file contains the fastlane.tools configuration");
                    outfile.WriteLine("# You can find the documentation at https://docs.fastlane.tools");
                    outfile.WriteLine("# For a list of all available actions, check out");
                    outfile.WriteLine("#     https://docs.fastlane.tools/actions;");
                    outfile.WriteLine("# For a list of all available plugins, check out");
                    outfile.WriteLine("#     https://docs.fastlane.tools/plugins/available-plugins");
                    outfile.WriteLine("");
                    outfile.WriteLine("# Uncomment the line if you want fastlane to automatically update itself");
                    outfile.WriteLine("# update_fastlane");
                    outfile.WriteLine("");
                    outfile.WriteLine("default_platform(:ios)");
                    outfile.WriteLine("");
                    outfile.WriteLine("platform :ios do");
                    outfile.WriteLine("  ################################################################################");
                    outfile.WriteLine("  desc 'Push a new beta build to TestFlight'");
                    outfile.WriteLine("  ################################################################################");
                    outfile.WriteLine("  lane :beta do");
                    outfile.WriteLine("");
                    outfile.WriteLine("    match(");
                    outfile.WriteLine("    app_identifier: [");
                    outfile.WriteLine("        'com.brainbow.speedoku'");
                    outfile.WriteLine("        ],");
                    outfile.WriteLine("    type: 'adhoc',");
                    outfile.WriteLine("    readonly: true,");
                    outfile.WriteLine("    clone_branch_directly: true,");
                    outfile.WriteLine("    shallow_clone: true");
                    outfile.WriteLine("    )");
                    outfile.WriteLine("");
                    outfile.WriteLine("    gym(scheme: 'Unity-iPhone')");
                    outfile.WriteLine("    crashlytics(");
                    outfile.WriteLine("    groups: 'peakteam',");
                    outfile.WriteLine("    api_token: '5b69d6b3859e684d6008664be7aed667b6f458ef',");
                    outfile.WriteLine("    build_secret: '5d951665c2deaa9344d6f767e8b67b36d6db8a7866bbe4967ee2c61f6f835699'");
                    outfile.WriteLine("  )");
                    outfile.WriteLine("  end");
                    outfile.WriteLine("  ################################################################################");
                    outfile.WriteLine("  desc 'Setup certificates and provisioning profiles'");
                    outfile.WriteLine("  ################################################################################");
                    outfile.WriteLine("  lane :certificates do");
                    outfile.WriteLine("");
                    outfile.WriteLine("    match(");
                    outfile.WriteLine("    app_identifier: [");
                    outfile.WriteLine("        'com.brainbow.numberssaga',");
                    outfile.WriteLine("        'com.brainbow.wordpolice',");
                    outfile.WriteLine("        'com.brainbow.quixellogic',");
                    outfile.WriteLine("        'com.brainbow.speedoku'");
                    outfile.WriteLine("        ],");
                    outfile.WriteLine("    type: 'development',");
                    outfile.WriteLine("    clone_branch_directly: true,");
                    outfile.WriteLine("    shallow_clone: true");
                    outfile.WriteLine("    )");
                    outfile.WriteLine("");
                    outfile.WriteLine("    match(");
                    outfile.WriteLine("    app_identifier: [");
                    outfile.WriteLine("        'com.brainbow.numberssaga',");
                    outfile.WriteLine("        'com.brainbow.wordpolice',");
                    outfile.WriteLine("        'com.brainbow.quixellogic',");
                    outfile.WriteLine("        'com.brainbow.speedoku'");
                    outfile.WriteLine("        ],");
                    outfile.WriteLine("    type: 'appstore',");
                    outfile.WriteLine("    readonly: true,");
                    outfile.WriteLine("    clone_branch_directly: true,");
                    outfile.WriteLine("    )");
                    outfile.WriteLine("");
                    outfile.WriteLine("    match(");
                    outfile.WriteLine("    app_identifier: [");
                    outfile.WriteLine("        'com.brainbow.numberssaga',");
                    outfile.WriteLine("        'com.brainbow.wordpolice',");
                    outfile.WriteLine("        'com.brainbow.quixellogic',");
                    outfile.WriteLine("        'com.brainbow.speedoku'");
                    outfile.WriteLine("        ],");
                    outfile.WriteLine("    type: 'adhoc',");
                    outfile.WriteLine("    force: false,");
                    outfile.WriteLine("    clone_branch_directly: true,");
                    outfile.WriteLine("    )");
                    outfile.WriteLine("");
                    outfile.WriteLine("  end");
                    outfile.WriteLine("end");
                    outfile.WriteLine("");
                }

                AssetDatabase.Refresh();
            }

            string matchfilePath = projectPath + "/fastlane/Matchfile";

            if (File.Exists(matchfilePath) == false)
            { // do not overwrite
                Debug.Log("Create Match file");
                using (StreamWriter outfile =
                    new StreamWriter(matchfilePath))
                {
                    outfile.WriteLine("git_url 'https://github.com/brainbow/certificates.git'");
                    outfile.WriteLine("");
                    outfile.WriteLine("team_id 'TGZW3NA5J5'");
                    outfile.WriteLine("");
                    outfile.WriteLine("# app_identifier 'tools.fastlane.app'");
                    outfile.WriteLine("username 'ops@peak.net' # Your Apple Developer Portal username");
                    outfile.WriteLine("");
                }

                AssetDatabase.Refresh();
            }

            string appPath = projectPath + "/fastlane/Appfile";

            if (File.Exists(appPath) == false)
            { // do not overwrite
                Debug.Log("Create App file");
                using (StreamWriter outfile =
                    new StreamWriter(appPath))
                {
                    outfile.WriteLine("app_identifier('com.brainbow.quixellogic') # The bundle identifier of your app");
                    outfile.WriteLine("apple_id('ops@peak.net') # Your Apple email address");
                    outfile.WriteLine("");
                    outfile.WriteLine("itc_team_id('1334403') # App Store Connect Team ID");
                    outfile.WriteLine("team_id('TGZW3NA5J5') # Developer Portal Team IDe");
                    outfile.WriteLine("");
                    outfile.WriteLine("# For more information about the Appfile, see:");
                    outfile.WriteLine("#     https://docs.fastlane.tools/advanced/#appfile");
                    outfile.WriteLine("");
                }

                AssetDatabase.Refresh();
            }

            Debug.Log("[PostProcessBuild] Fastfile files created.");
            //#endif
        }
    }
}