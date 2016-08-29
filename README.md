# Simple Storage

Simple storage library provides simple file operations which is easy to unit test and mock. The library is simple and effective. It provides the basic operations that you need to handle files. For now it is limited to text files. Extensions will be added in the future.

I have created this library after a need to test a library that uses file system access to read and write files. I find it is hard to test this library without isolating the file system operations in an interface which can be mocked. For the easy of testing, I have provided an implementation of this interface that only use memory to store the directories and files without the need for physical file access. It is easier to manage. No need to prepare and copy files and directories for unit tests to run.

For more information, check the wiki.
