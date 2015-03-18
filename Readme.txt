===============================================================
======================== XML Converter ========================
===============================================================
Sometimes we need to configure XML files from generated files.


What we need to do:
We need to be able to generate a templated configuration file from generated other XML files.


Project file hierarchy

|_ Mapper
| |_ Mapper.xml (Which nodes to update and how)
| |_ Mapper.xsd (Schema for mapper.xml to ensure structure aligns with CSS classes)
| |_ Template.xml (The output file without the updates)
|_ Source Files
| |_ generated_contentA.xml
| |_ generated_contentB.xml
|_ Output
  |_ output.xml (The template updated by the mapper [Constants and generated XML files])


How to use the application:
1. Create a ouput template XML file
2. Make a list of all the updates that needs to happen (manual and from other XML files)
   Capture this mapping info into the Mapper.XML
3. Copy the source XML files into the source folder
4. RUn the XML converter and it will generate the output.xml file


Items to explain more:
- Configuring the file names (E.g. Output.xml)
- Capturing a Mapper entree
- Troubleshooting issues