using System;
using System.IO;

namespace XNotepad.Core.Utilities
{
    public static class FileUtilities
    {
        public static string ExtractFileNameFromFullPath(string path)
        {
            if (!ValidateFilePath(path))
            {
                return string.Empty;
            }

            var indexLastSlash = path.LastIndexOfAny(new char[] { '\\', '/' });

            if (indexLastSlash < 0)
            {
                return path;
            }
            else
            {
                return path.Substring(indexLastSlash + 1);
            }
        }

        public static bool ValidateFilePath(string path, string relativePath = "", string extension = "", bool requireExists = false)
        {
            if (path is null)
                return false;

            var root = Path.GetPathRoot(path);

            var noRootPath = root is null ? path : path.Substring(root.Length);

            // Check if it contains any Invalid Characters.
            foreach (var invalidChar in Path.GetInvalidPathChars())
            {
                if (noRootPath.IndexOf(invalidChar) >= 0)
                    return false;
            }

            var fileName = Path.GetFileName(path);

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                if (fileName.IndexOf(invalidChar) >= 0)
                    return false;
            }

            try
            {
                // Exceptions from FileInfo Constructor:
                //   System.ArgumentNullException:
                //     fileName is null.
                //
                //   System.Security.SecurityException:
                //     The caller does not have the required permission.
                //
                //   System.ArgumentException:
                //     The file name is empty, contains only white spaces, or contains invalid characters.
                //
                //   System.IO.PathTooLongException:
                //     The specified path, file name, or both exceed the system-defined maximum
                //     length. For example, on Windows-based platforms, paths must be less than
                //     248 characters, and file names must be less than 260 characters.
                //
                //   System.NotSupportedException:
                //     fileName contains a colon (:) in the middle of the string.
                FileInfo fileInfo = new FileInfo(path);

                // Exceptions using FileInfo.Length:
                //   System.IO.IOException:
                //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
                //     directory.
                //
                //   System.IO.FileNotFoundException:
                //     The file does not exist.-or- The Length property is called for a directory.
                bool throwEx = fileInfo.Length == -1;

                // Exceptions using FileInfo.IsReadOnly:
                //   System.UnauthorizedAccessException:
                //     Access to fileName is denied.
                //     The file described by the current System.IO.FileInfo object is read-only.-or-
                //     This operation is not supported on the current platform.-or- The caller does
                //     not have the required permission.
                throwEx = fileInfo.IsReadOnly;

                if (!string.IsNullOrEmpty(extension))
                {
                    // Validate the extension of the file.
                    if (Path.GetExtension(path).Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Trim the Library Path
                        path = path.Trim();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;

                }
            }
            catch (ArgumentNullException)
            {
                //   System.ArgumentNullException:
                //     fileName is null.
            }
            catch (System.Security.SecurityException)
            {
                //   System.Security.SecurityException:
                //     The caller does not have the required permission.
            }
            catch (ArgumentException)
            {
                //   System.ArgumentException:
                //     The file name is empty, contains only white spaces, or contains invalid characters.
            }
            catch (UnauthorizedAccessException)
            {
                //   System.UnauthorizedAccessException:
                //     Access to fileName is denied.
            }
            catch (PathTooLongException)
            {
                //   System.IO.PathTooLongException:
                //     The specified path, file name, or both exceed the system-defined maximum
                //     length. For example, on Windows-based platforms, paths must be less than
                //     248 characters, and file names must be less than 260 characters.
            }
            catch (NotSupportedException)
            {
                //   System.NotSupportedException:
                //     fileName contains a colon (:) in the middle of the string.
            }
            catch (FileNotFoundException)
            {
                if (!requireExists)
                    return true;

                // System.FileNotFoundException
                //  The exception that is thrown when an attempt to access a file that does not
                //  exist on disk fails.
            }
            catch (IOException)
            {
                //   System.IO.IOException:
                //     An I/O error occurred while opening the file.
            }
            catch (Exception)
            {
                // Unknown Exception. Might be due to wrong case or nulll checks.
            }

            return false;
        }
    }
}
