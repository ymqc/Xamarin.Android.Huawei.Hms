#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Cake Addins
#addin nuget:?package=Cake.FileHelpers&version=3.3.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var artifacts = new [] {
    new Artifact {
        NativeVersion = "1.3.3.300",
        NugetSuffix = "",
        AarName = "tasks",
        ProjectName = "HmsTasks",
        DownloadUrlPrefix = "hmf"
    },

    new Artifact {
        NativeVersion = "4.0.2.300",
        NugetSuffix = "",
        AarName = "push",
        ProjectName = "HmsPush"
    },
    new Artifact {
        NativeVersion = "4.0.1.301",
        NugetSuffix = "",
        AarName = "opendevice",
        ProjectName = "HmsOpenDevice"
    },
    new Artifact {
        NativeVersion = "4.0.1.300",
        NugetSuffix = "",
        AarName = "maps",
        ProjectName = "HmsMaps"
    },
    new Artifact {
        NativeVersion = "4.0.1.300",
        NugetSuffix = "",
        AarName = "maps-base",
        ProjectName = "HmsMapsBase"
    },
    new Artifact {
        NativeVersion = "4.0.1.300",
        NugetSuffix = "",
        DownloadUrl = "https://githubstore.blob.core.windows.net/huawei-aar/clustering-release.aar?sp=r&st=2020-08-03T15:33:43Z&se=2022-08-03T23:33:43Z&spr=https&sv=2019-12-12&sr=b&sig=mLCbWNm1MiTMbsqH2i5vidO61BXaydpUnI420TfhW2c%3D",
        AarName = "clustering",
        ProjectName = "HmsMapsClustering"
    },
    new Artifact {
        NativeVersion = "4.0.2.300",
        NugetSuffix = "",
        AarName = "location",
        ProjectName = "HmsLocation"
    },
    new Artifact {
        NativeVersion = "1.0.13.300",
        NugetSuffix = "",
        AarName = "dynamic-api",
        ProjectName = "HmsDynamicApi"
    },
    new Artifact {
        NativeVersion = "1.0.0.300",
        NugetSuffix = "",
        AarName = "agconnect-core",
        ProjectName = "HmsAgconnectCore",
        DownloadUrlPrefix = "agconnect"
    },

    new Artifact {
        NativeVersion = "1.0.0.300",
        NugetSuffix = "",
        AarName = "agconnect-credential",
        ProjectName = "HmsAgconnectCredential",
        DownloadUrlPrefix = "agconnect"
    },

    new Artifact {
        NativeVersion = "1.0.0.300",
        NugetSuffix = "",
        AarName = "agconnect-https",
        ProjectName = "HmsAgconnectHttps",
        DownloadUrlPrefix = "agconnect"
    },

    new Artifact {
        NativeVersion = "1.0.0.300",
        NugetSuffix = "",
        AarName = "datastore-annotation",
        ProjectName = "HmsDataStoreAnnotation",
        DownloadUrlPrefix = "agconnect",
        JarExtension = "jar"
    },

    new Artifact {
        NativeVersion = "1.0.0.300",
        NugetSuffix = "",
        AarName = "datastore-core",
        ProjectName = "HmsDataStoreCore",
        DownloadUrlPrefix = "agconnect",
    },

    new Artifact {
        NativeVersion = "5.0.0.301",
        NugetSuffix = "",
        AarName = "hianalytics",
        ProjectName = "HmsHiAnalytics",
    },

    new Artifact {
        NativeVersion = "4.0.0.300",
        NugetSuffix = "",
        AarName = "base",
        ProjectName = "HmsBase"
    },
    new Artifact {
        NativeVersion = "4.0.2.300",
        NugetSuffix = "",
        AarName = "network-common",
        ProjectName = "HmsNetworkCommon"
    },
    new Artifact {
        NativeVersion = "4.0.2.300",
        NugetSuffix = "",
        AarName = "network-grs",
        ProjectName = "HmsNetworkGrs"
    },

     new Artifact {
        NativeVersion = "2.0.6.300",
        NugetSuffix = "",
        AarName = "update",
        ProjectName = "HmsUpdate"
    },

    new Artifact {
        NativeVersion = "2.0.1.300",
        NugetSuffix = "",
        AarName = "ml-computer-commonutils-inner",
        ProjectName = "HmsMLCommonUtils",
    },

    new Artifact {
        NativeVersion = "2.0.1.300",
        NugetSuffix = "",
        AarName = "ml-computer-ha-inner",
        ProjectName = "HmsMLHa",
    },

    new Artifact {
        NativeVersion = "2.0.1.300",
        NugetSuffix = "",
        AarName = "ml-computer-camera-inner",
        ProjectName = "HmsMLCamera",
    },

    new Artifact {
        NativeVersion = "2.0.1.300",
        NugetSuffix = "",
        AarName = "ml-computer-agc-inner",
        ProjectName = "HmsMLAgc",
    },

    new Artifact {
        NativeVersion = "2.0.1.300",
        NugetSuffix = "",
        AarName = "ml-computer-sdkbase-inner",
        ProjectName = "HmsMLBase",
    },

    new Artifact {
        NativeVersion = "1.2.1.300",
        NugetSuffix = "",
        AarName = "scan",
        ProjectName = "HmsScan",
    }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Downloads")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        var downloadUrl = string.Format(artifact.DownloadUrl, artifact.DownloadUrlPrefix, artifact.AarName, artifact.NativeVersion, artifact.JarExtension);
        var jarPath = string.Format(artifact.JarPath, artifact.ProjectName, artifact.AarName, artifact.NativeVersion, artifact.JarExtension);

        DownloadFile(downloadUrl, jarPath);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("**/*/packages");

    CleanDirectory("./nugets/*");

    var nugetPackages = GetFiles("./nugets/*.nupkg");

    foreach (var package in nugetPackages)
    {
        DeleteFile(package);
    }
});

Task("UpdateVersion")
    .Does(() => 
{
    foreach(var artifact in artifacts) {
        ReplaceRegexInFiles(artifact.AssemblyInfoPath, "\\[assembly\\: AssemblyVersion([^\\]]+)\\]", string.Format("[assembly: AssemblyVersion(\"{0}\")]", artifact.NativeVersion));
    }
});

Task("Build")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetRestore(artifact.SolutionPath);
        MSBuild(artifact.SolutionPath, settings => {
            settings.ToolVersion = MSBuildToolVersion.VS2019;
            settings.SetConfiguration(configuration);
            settings.WithTarget("Rebuild");
        });

        break;
    }
});

Task("Pack")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetPack(artifact.Csproj, new NuGetPackSettings {
            Version = artifact.Version,
            Dependencies = artifact.Dependencies,
            ReleaseNotes = artifact.ReleaseNotes,
            IncludeReferencedProjects = true,
            Properties = new Dictionary<string, string>
            {
                { "Configuration", "Release" }
            },
            OutputDirectory = "./nugets"
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
   // .IsDependentOn("Downloads")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Build")
    .IsDependentOn("Pack")
   ;

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

class Artifact {

    public string Version
    {
        get { return NativeVersion + NugetSuffix; }
    }

    public string ProjectName {get; set; }

    public string NugetSuffix { get; set; }

    public string NativeVersion { get; set; }

    public string AssemblyInfoPath
    {
        get
        {
            return string.Format("./{0}/Properties/AssemblyInfo.cs", ProjectName);
        }
    }

    public string SolutionPath { get; set; } = "./Hms.sln";

    public string DownloadUrlPrefix { get;set;} = "hms";

    public string DownloadUrl { get; set; } = "https://developer.huawei.com/repo/com/huawei/{0}/{1}/{2}/{1}-{2}.{3}";

    public string JarExtension { get;set;} = "aar";

    public string JarPath { get; } = "./{0}/Jars/{1}-{2}.{3}";

    public string AarName {get; set;}

    public string NuspecPath => string.Format("./{0}/{0}.nuspec", ProjectName);

    public string Csproj
    {
        get
        {
            return NuspecPath.Replace("nuspec", "csproj");
        }
    }

    public string[] _releaseNotes = null;
    public string[] ReleaseNotes 
    {
        get
        {
            if (_releaseNotes == null)
            {
                return new string[]
                {
                    string.Format("Huawei Hms {0} v{1}", ProjectName, NativeVersion)
                };
            }

            return _releaseNotes;
        }
        set
        {
            _releaseNotes = value;
        }

    }


    private NuSpecDependency[] _dependencies = new NuSpecDependency[0];
    public NuSpecDependency[] Dependencies
    {
        get { return _dependencies; }
        set { _dependencies = value; }
    }
}