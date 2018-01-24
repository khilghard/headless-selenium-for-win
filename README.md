# Headless Selenium for Windows

[![Build status](https://ci.appveyor.com/api/projects/status/v5mtxvfvb2necfkb?svg=true)](https://ci.appveyor.com/project/kybu/headless-selenium-for-win)

It is quite inconvenient when a browser window pops up when running Selenium
tests. It might cause tests to fail because the browser window needs to hold user
input focus. That is the case with IE at least.

In the Linux world, this is solved by running browsers in a virtual frame-buffer.
A similar approach can be taken on the Windows platform as well.

Contrary to popular belief, Windows is fully multi-user OS. On top of
that, it supports virtual desktops even though it is not very often used as compared
to Linux desktop environments.

This application uses virtual desktops to run web browsers so that they
do not disturb the main user desktop.

It can be used by any language supported by Selenium Webdriver (Java, C#,
Ruby, ...).

# Download

Binaries can be downloaded from https://github.com/kybu/headless-selenium-for-win/releases 

# Firefox & Chrome

Selenium uses "drivers" to control web browsers. They are standalone
executables driving browsers. `headless_ie_selenium.exe` by default looks
for the IE driver in `PATH`, but it can be instructed to use other drivers
as well. All command line arguments are forwarded to the driver, so the
`HEADLESS_DRIVER` environment variable is used to specify the driver. Put
the driver in one of the `PATH` directories.

Set the `HEADLESS_DRIVER` environment variable to `geckodriver.exe` for
headless Firefox.

# Internet Explorer

When using IE, please pay full attention to configure the driver correctly. All
necessary details can be found at https://code.google.com/p/selenium/wiki/InternetExplorerDriver
in the 'Required Configuration' section and at http://heliumhq.com/docs/internet_explorer

## Setup

Selenium uses a standalone executable called `IEDriverServer.exe` to drive the IE browser window.
Selenium has to be instructed to use the 'headless_ie_selenium.exe' executable to run tests
headlessly.

`headless_ie_selenium.exe` creates a virtual desktop and runs `IEDriverServer.exe` inside of it.
Any command line parameters are passed on to the IE driver.

`IEDriverServer.exe` has to be stored in the searchable path.

New virtual desktop name is `HeadlessDesktop`. If you want to create a unique desktop name each time
the `headless_ie_selenium.exe` executable is run, variable `HEADLESS_UNIQUE` has to exist in the environment.
Its value does not matter, unique desktop name will be generated as long as this variable is present.

An addition was made recently that allows for the driver path and unique value to be set at the command line:
  1. -unique
  2. -driver MicrosoftWebDriver.exe

They are used like this:
  - headless_ie_selenium.exe -unique -driver MicrosoftWebDriver.exe --host=127.0.0.1 --verbose --port=51131

## Basic Ruby example

Following example uses www.google.com search to retrieve weather in London.

```ruby
require 'selenium-webdriver'

Dir.chdir(File.dirname $0)

# Generate unique desktop name each run
ENV['HEADLESS_UNIQUE'] = 1

# Find the Headless IE binary.
headlessBinary =
  if File.exists?('../Debug/headless_ie_selenium.exe')
    '../Debug/headless_ie_selenium.exe'
  elsif File.exists?('../Release/headless_ie_selenium.exe')
    '../Release/headless_ie_selenium.exe'
  else
    raise 'Cound not find headless_ie_selenium.exe!'
  end

puts "Headless IE binary found at: #{headlessBinary}"

# The most important part!
Selenium::WebDriver::IE.driver_path = headlessBinary

driver = Selenium::WebDriver.for :ie
driver.get 'https://www.google.com'

sleep 2

searchInput = driver.find_element :name => 'q'

searchInput.send_keys "london weather"
searchInput.submit

sleep 2

weather = driver.find_element :id => 'wob_wc'

puts weather.text

driver.quit
```

Output might be something like this. 'Sunny Weather' can be retrieved
only on rare occasions:

    Headless IE binary: ../Debug/headless_ie_selenium.exe
    London, UK
    Tuesday 11:00
    Partly Cloudy
    19
    °C | °F
    Precipitation: 3%
    Humidity: 69%
    Wind: 0 mph
    TemperaturePrecipitationWind
    1921211915141311
    12:0015:0018:0021:0000:0003:0006:0009:00
    Tue
    22°12°
    Wed
    21°11°
    Thu
    22°12°
    Fri
    22°13°
    Sat
    23°13°
    Sun
    23°16°
    Mon
    22°14°
    Tue
    22°13°

# Executables

'Headless Selenium for Windows' comes with these two binaries: `desktop_utils.exe` and `headless_ie_selenium.exe`

## headless_ie_selenium.exe

This executable runs IE Webdriver `IEDriverServer.exe` headlessly. It is
meant to be used by Selenium Webdriver library instead of IE Webdriver.
Other Selenium drivers can be used by specifying `HEADLESS_DRIVER`
environment variable.

## desktop_utils.exe

Generic Windows virtual desktop utility. It can run applications in virtual desktops, etc ...

Command line options:

    Desktop utils v1.4, Peter Vrabel (c) 2014-2017:
      -h [ --help ]               Feeling desperate?
      -r [ --run ] arg            Command to run headlessly.
      -n [ --desktop ] arg        Set the headless desktop name. Used with '--run'.
                                  Optional, default = HeadlessDesktop
      -l [ --list ]               List available desktops of current Window
                                  station.
      -s [ --switch-to ] arg      Switch to a desktop. Takes a desktop name from
                                  the list of desktops.
      -t [ --switch-to-default ]  Switch to the default desktop. Can be used if you
                                  are being stranded.
      -x [ --no-explorer ]        Don't run explorer in the created desktop


# Build Process

## Boost 1.66.0

https://stackoverflow.com/questions/13042561/fatal-error-lnk1104-cannot-open-file-libboost-system-vc110-mt-gd-1-51-lib/13042696

1. First Unzip the boost library to any directory of your choice. I recommend c:\Program Files.
1. Open your visual C++.
1. Create a new project.
1. Right click on the project.
1. Click on property.
1. Click on C/C++.
1. Click on general.
1. Select additional include library.
1. Include the library destination. e.g. c:\boost_1_66_0.
1. Click on pre-compiler header.
1. Click on create/use pre-compiled header.
1. Select not using pre-compiled header.
1. Then go over to the link library were you experienced your problems.

1. Go to were the extracted file was c:\boost_1_66_0.
1. Click on booststrap.bat (don't bother to type on the command window just wait and don't close the window that is the place I had my problem that took me two weeks to solve. After a while the booststrap will run and produce the same file, but now with two different names: b2, and bjam.
1. Click on b2 and wait it to run.
1. Click on bjam and wait it to run. Then a folder will be produce called stage.
1. Right click on the project.
1. Click on property.
1. Click on linker.
1. Click on general.
1. Click on include additional library directory.
1. Select the part of the library e.g. c:\boost_1_66_0\stage\lib.
1. And you are good to go!


# Technical details

Tested on Win 7, 10.

Supported browsers: IE, Firefox, Chrome.

Developed using VS 2017 Enterprise, Boost 1.66 and a bit of system level
programming skills.

# License

GPLv3, Copyright 2014-2017 Peter Vrabel

