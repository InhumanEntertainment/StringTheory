using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;


public class MobclixMenuItem : MonoBehaviour
{
	private const string _disabledPrefix = "DISABLED";
	private const string _admobScriptName = "PostprocessBuildPlayer_Admob";
	private const string _millenialScriptName = "PostprocessBuildPlayer_Millenial";
	
	void Update()
	{
		Debug.Log( "update" );
	}
	
	
	[MenuItem( "Prime31/Mobclix Admob Support/Enable", true )]
	static bool admobEnableCheck()
	{
		return !isAdmobEnabled();
	}
	
	
	[MenuItem( "Prime31/Mobclix Admob Support/Disable", true )]
	static bool admobDisableCheck()
	{
		return isAdmobEnabled();
	}


	[MenuItem( "Prime31/Mobclix Millenial Support/Enable", true )]
	static bool millenialEnableCheck()
	{
		return !isMilenialEnabled();
	}
	
	
	[MenuItem( "Prime31/Mobclix Millenial Support/Disable", true )]
	static bool millenialDisableCheck()
	{
		return isMilenialEnabled();
	}

	
	[MenuItem( "Prime31/Mobclix Admob Support/Enable" )]
	static void admobEnable()
	{
		enableAdmobSupport( true );
	}
	
	
	[MenuItem( "Prime31/Mobclix Admob Support/Disable" )]
	static void admobDisable()
	{
		enableAdmobSupport( false );
	}


	[MenuItem( "Prime31/Mobclix Millenial Support/Enable" )]
	static void millenialEnable()
	{
		enableMillenialSupport( true );
	}
	
	
	[MenuItem( "Prime31/Mobclix Millenial Support/Disable" )]
	static void millenialDisable()
	{
		enableMillenialSupport( false );
	}
	
	
	
	
	static bool isAdmobEnabled()
	{
		string admobScript = getFullPathToAdmobScript();
		
		if( admobScript == null )
		{
			throw new Exception( "Could not find Admob post process script" );
		}
		
		// grab just the file name
		string fileName = Path.GetFileName( admobScript );
		
		return !( fileName.StartsWith( _disabledPrefix ) );
	}
	

	static bool isMilenialEnabled()
	{
		string millenialScript = getFullPathToMillenialScript();
		
		if( millenialScript == null )
		{
			throw new Exception( "Could not find Millenial post process script" );
		}
		
		// grab just the file name
		string fileName = Path.GetFileName( millenialScript );
		
		return !( fileName.StartsWith( _disabledPrefix ) );
	}
	
	
	static string getFullPathToAdmobScript()
	{
		// grab a hook to the Editor directory
		string editorPath = Path.Combine( Environment.CurrentDirectory, "Assets/Editor" );
		
		if( Directory.Exists( editorPath ) )
		{
			var dirInfo = new DirectoryInfo( editorPath );
			
			// find the Mobclix post process build script and see if it is enable
			foreach( var file in dirInfo.GetFiles() )
			{
				if( file.Name.EndsWith( _admobScriptName ) )
					return file.FullName;
			}
		}
		
		return null;
	}


	static string getFullPathToMillenialScript()
	{
		// grab a hook to the Editor directory
		string editorPath = Path.Combine( Environment.CurrentDirectory, "Assets/Editor" );
		
		if( Directory.Exists( editorPath ) )
		{
			var dirInfo = new DirectoryInfo( editorPath );
			
			// find the Mobclix post process build script and see if it is enable
			foreach( var file in dirInfo.GetFiles() )
			{
				if( file.Name.EndsWith( _millenialScriptName ) )
					return file.FullName;
			}
		}
		
		return null;
	}

	
	static void enableAdmobSupport( bool enable )
	{
		// grab the source file with path
		string sourceAdmobScript = getFullPathToAdmobScript();
		
		// figure out the new file with path
		string newFilename = ( enable ) ? _admobScriptName : _disabledPrefix + _admobScriptName;
		string destAdmobScript = Path.Combine( Environment.CurrentDirectory, "Assets/Editor/" + newFilename );
		
		if( !File.Exists( sourceAdmobScript ) )
		{
			throw new Exception( "Could not find Admob script" );
		}
		
		// rename the file
		try
		{
			FileUtil.MoveFileOrDirectory( sourceAdmobScript, destAdmobScript );
		}
		catch( Exception e )
		{
			EditorUtility.DisplayDialog( "An error occurred renaming the PostprocessBuildPlayer_Admob file.  Please ensure the directory (Assets/Editor) is writeable.", e.Message, "OK" );
		}
		
		// if we disabled, show a message to be sure the Xcode project gets wiped else to add the publisher ID
		if( !enable )
			EditorUtility.DisplayDialog( "Important Notice", "Please be sure to do a Build then Replace for the changes to take effect", "OK" );
		else
			EditorUtility.DisplayDialog( "Important Notice", "Please be sure to add your Admob publisher ID to the AdmobPublisherId.txt file", "OK" );
	}


	static void enableMillenialSupport( bool enable )
	{
		// grab the source file with path
		string sourceMillenialScript = getFullPathToMillenialScript();
		
		// figure out the new file with path
		string newFilename = ( enable ) ? _millenialScriptName : _disabledPrefix + _millenialScriptName;
		string destMillenialScript = Path.Combine( Environment.CurrentDirectory, "Assets/Editor/" + newFilename );
		
		if( !File.Exists( sourceMillenialScript ) )
		{
			throw new Exception( "Could not find Admob script" );
		}
		
		// rename the file
		try
		{
			FileUtil.MoveFileOrDirectory( sourceMillenialScript, destMillenialScript );
		}
		catch( Exception e )
		{
			EditorUtility.DisplayDialog( "An error occurred renaming the PostprocessBuildPlayer_Millenial file.  Please ensure the directory (Assets/Editor) is writeable.", e.Message, "OK" );
		}
		
		// if we disabled, show a message to be sure the Xcode project gets wiped else to add the publisher ID
		if( !enable )
			EditorUtility.DisplayDialog( "Important Notice", "Please be sure to do a Build then Replace for the changes to take effect", "OK" );
	}

}
