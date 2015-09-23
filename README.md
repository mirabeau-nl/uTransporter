# uTransporter (BETA)

uTransporter is a code-first tool for Umbraco 7+. When installed with NuGet a new dashboard section is added in the back-office. Here developers can choose the right action like:
Sync, Dry run, Generate.

* Generate: This creates .cs files of your existing document types and templates.
* Dry run: Make a copy of the current Umbraco DB and runs the sync on it. After completion, the dry run DB gets deleted, this way you can safely try out your sync.
* Sync: Imports the .cs classes into your current Umbraco CMS.
* Remove: Removes all your document types and templates from Umbraco.

### Getting started ###

Before you can run the application in Visual Studio and start developing it, you will need to install the Umbraco CMS. A guide about the installation can be found here [Umbraco Manual Installation][]

[Umbraco Manual Installation]: http://our.umbraco.org/documentation/Installation/install-umbraco-manually

### How do I get set up? ###

Checkout the source code in a directory of your choice. Add the project to your newly created Umbraco solution.

Build the project

If you want to use it without development just get the nuget package:

	Install-Package Mirabeau.uTransporter
    
### Umbraco 7 back-office plugin ###

The source-code for the back-office plugin is located in ~/Mirabeau.uTransporter/Backoffice/.
Changes to the plugin can be made directly in this directory.
If you want to use this plugin in your Umbraco, copy the directory "uTransporter" into the App_Plugins directory in Umbraco. Afterwards,copy the content of the uTransporterController.cs.pp into a new file called uTransporterController.cs.
The last step is to apply the Dashboard.config.transform to your current Dashboard.config

### Testing ###

uTransporter comes with a set of Unit Tests, to run the tests in Visual Studio:

* In the **TEST** menu,  hover on **Run**, and then choose the option you want.

Alternatively, you can use the Test Explorer in Visual Studio:

1. Click the **TEST** menu, hover on **Windows**, and then click **Test Explorer**.
2. In the Test Explorer pane, click **Run All** to run all the tests, or choose a different option from the **Run** menu.

For more information about running unit tests, see the topic [Running Unit Tests with Test Explorer](http://msdn.microsoft.com/en-us/library/hh270865.aspx) on the Microsoft Developer Network (MSDN).

### Contribution guidelines ###

* Comments, methods and variables in english.
* Create unittests where possible.
* Try to stick to the existing coding style.
* Give a short description in the pull request about what you're doing and why.