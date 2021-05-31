# MealPlanner

This program is being developed to help the owner of a small restaurant with creating his weekly meal plans. The program allows him to select 
the individual courses of a single meal and click them together for each day of the week. The resulting meal plan can then be 
exported into a predefined and formatted Excel template. In addition to that, it can also be exported as a PDF file.

The owner of the restaurant readily admits to "not being very good with computers" and not being particularly interested in 
learning it at his age. As a result, some of the design requirements were to make the UI simple to use, and to not break his 
existing workflow (convoluted as it may be).

## Current state of the project

The project is essentially still a prototype. Nevertheless, the core functionality is there and it is in active use already.

## Planned improvements

* Cleaner MVVM architecture (possibly through use of the Prism framework)
* IoC through Dependency Injection
* Making data storage more robust by using JSON or SQlite (the current CSV files are more like a crutch)
* Improving on generating the PDF files
* Making the template accessible to the user (I'm currently using a XAML file, which is not very user friendly)
* Improving the design of the UI (I'm not a good UI designer)
