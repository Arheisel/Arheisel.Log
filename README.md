# Arheisel.Log
Simple Thread safe logging library

# How to use

Call the `Log.Start()` Function at the start of your execution to initialize the logger thread. It will create a Log folder in your executable's path if it doesn't already exist.
A log file with the current date will be added if it doesn's exist. Otherwise it will appent to it.

Call the `Log.Write(string type, string message)` from anywhere in your code to Log to the file, `type` can be any string. You can also call any of the pre-built log functions:

`Log.Debug(string message)` Note: Doesn't output anything on a Release build.

`Log.Info(string message)`

`Log.Warning(string message)`

`Log.Error(string message)`

`Log.Exception(Exception e)` Alias of `Log.Write(Exception e)`
