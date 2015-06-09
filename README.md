Browse
======

A Web Site Tester/Hack

We needed something at work to quickly verify that our custom code was still working on our web site whenever there was an update/upgrade.

This allows you to set the root of the web site, a pause (so you can login) then as many pages you would like to open up in a new tab/browser window to verify things are working.

It is currently mostly manual, no http status codes or anything like that.

App Icon used from [Earam](http://findicons.com/pack/2115/moonlight)

##Building
If you use GitHub for Windows it wil fetch the submodule(s) for you. If not the following should work.

```
git clone --recursive
```

This code was written to include the CC.Common.JSON assembly in the main executable. Because of this you will need to build the project in RELEASE first (this it what it links to).

After that everything should be hunky dory, good to go, bobs you're uncle, etc.
