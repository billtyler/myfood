# myfood app & hub - Solar Smart Greenhouse Using Vertical Aquaponic Towers

__Technical architecture overview:__
<a href="https://myfood.eu/wp-content/uploads/2017/03/myfood-solution-architecture.pdf">download the document</a>

__To compile flawlessly:__ 
- on Visual Studio 2015/2017, under the Package Manager Console, execute Update-Package -Reinstall
- download Telerik ASP MVC component trial to get Kendo.MVC.dll

__To deploy local database and dummy data:__
- on Visual Studio 2015, under the Package Manager Console, execute EntityFramework\Update-Database

__Hardware pre-requisite:__
- Raspberry Pi 2/3 with Win10 IOT
- RTC Pi Zero (<a href="https://www.abelectronics.co.uk/p/70/RTC-Pi-Zero">see the product</a>)
- Sigfox Extension Board (<a href="https://yadom.fr/carte-rpisigfox.html">see the product</a>)
- Humidity / Air Temperature Sensor (<a href="https://www.adafruit.com/product/1899">see the product</a>)
- Atlas Sensors (pH, Temp, DO, ORP) plus USB Board (<a href="https://www.atlas-scientific.com/product_pages/components/usb-iso.html">see the product</a>) and EZO circuits

__Material pre-requisite:__
- 22/14sq² tempered glass greenhouse (ie: S208 or S106 Model from <a href="http://www.acd.eu/en">ACD</a>) 
- 24X 5' Zipgrow towers (<a href="https://brightagrotech.com">see the product</a>) 
- Wooden structure (<a href="https://myfood.eu/wp-content/uploads/2017/03/myfood-family-plan.pdf">see the plan</a>)
- Water pump (ie: DCS-2000)
- PVC-U tubes
- 2X 600L food contact fish tanks (1150mm X 1550mm X 450mm)

__Greenhouse setup:__
<a href="https://myfood.eu/wp-content/uploads/2017/03/myfood-greenhouse-setup.pdf">see the plan</a>

__OpenData API__

__Get the Production Units list:__

 ```https://hub.myfood.eu/opendata/productionunits/ ```
 
 __Get the Production Units measures (last 1000):__
 
 ```https://hub.myfood.eu/opendata/productionunits/{id}/measures ```
