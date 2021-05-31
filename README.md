# MealPlanner

This program is being developed to help the owner of a small restaurant with creating his weekly meal plans. The program allows him to select 
the individual courses of a single meal and click them together for each day of the week. The resulting meal plan can then be 
exported into a predefined and formatted Excel template. In addition to that, it can also be exported as a PDF file.

The owner of the restaurant readily admits to "not being very good with computers" and not being particularly interested in 
learning these things at his age. As a result, some of the design requirements were to make the UI simple to use, and to not break his 
existing workflow (convoluted as it may be). This program simply provides a shortcut, while still leaving his old way of doing things intact. 
Another requirement was to make it work on hardware that is practically ancient by modern standards.

## Current state of the project

The project is essentially still a prototype. Nevertheless, the core functionality is there and it is in active use already.

## Planned improvements

* Cleaner MVVM architecture (possibly by useing the Prism framework)
* IoC through Dependency Injection
* Making data storage more robust by using JSON or SQlite (the current CSV files are more like a crutch)
* Improving on generating the PDF files
* Making the PDF template accessible to the user (I'm currently using a XAML file, which is not very user friendly)
* Improving the design of the UI (I'm not a good UI designer)

## What motivated me to do this project?

First and foremost, my motivation was to help. I just couldn't stand watching him struggle with his practically ancient laptop and his 
multiple Excel spreadsheets that he used, copying and pasting strings from one file to the other, paying attention to not overwrite any
formatting, etc. That prompted me to step in and make tho offer to create something to make it easier for him (free of charge of course). 
I had been looking for a project to familiarize myself with the WPF framework for quite a while now, and this seemed like as good as any
to get started. It also opens up the way for many more concepts for me to explore, including the MVVM architecture that is favoured by WPF, 
Dependency Injection, and persistent data storage. And while this is not strictly speaking a CRUD application, it could be turned into one 
with relative ease.
