﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfTreeView
{
  /// <summary>
  /// A helper class to query information about directories
  /// </summary>
  public static class DirectoryStructure
  {
    /// <summary>
    /// Gets all logical drives on the computer
    /// </summary>
    /// <returns></returns>
    public static List<DirectoryItem> GetLogicalDrives()
    {
      // Get Every Logical drive on the machine
      return Directory.GetLogicalDrives().Select(drive => new DirectoryItem { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
    }

    /// <summary>
    /// Gets the directories top-level content
    /// </summary>
    /// <param name="fullPath">The full path to the directory</param>
    /// <returns></returns>
    public static List<DirectoryItem> GetDirectoryContents(string fullPath)
    {
      //#region Initial Checks
      //var item = (TreeViewItem)sender;

      //// If the item only contains the dummy data
      //if (item.Items.Count != 1 || item.Items[0] != null)
      //{
      //  return;
      //}

      //// Clear dummy data
      //item.Items.Clear();

      //// Get full path
      //var fullPath = (string)item.Tag;

      //#endregion

      // Create empty list
      var items = new List<DirectoryItem>();

      #region Get Folders

      // Try and get directories from the folder
      // ignoring any issues doing so
      try
      {
        var dirs = Directory.GetDirectories(fullPath);

        if (dirs.Length > 0)
        {
          items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder }));
        }
      }
      catch
      { }

      #endregion

      #region Get Files

      // Try and get files from the folder
      // ignoring any issues doing so
      try
      {
        var fs = Directory.GetFiles(fullPath);

        if (fs.Length > 0)
        {
          items.AddRange(fs.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File }));
        }
      }
      catch
      { }

      #endregion

      return items;
    }

    #region Helpers

    /// <summary>
    /// Find the file or folder name from a full path
    /// </summary>
    /// <param name="path">The full path</param>
    /// <returns></returns>
    public static string GetFileFolderName(string path)
    {
      // C:\Something\a folder
      // C:\Something\a file.png
      // a file file.png

      // If we have no path, return empty
      if (string.IsNullOrEmpty(path))
      {
        return string.Empty;
      }

      // Make all slashes back slashes
      var normalizedPath = path.Replace('/', '\\');

      // Find the last backslash in the path
      var lastIndex = normalizedPath.LastIndexOf('\\');

      // If we don't find a backslash, return the path itself
      if (lastIndex <= 0)
      {
        return path;
      }

      // Return the name after the last backslash
      return path.Substring(lastIndex + 1);
    }

    #endregion
  }
}
