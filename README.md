# CSharp_ASP.NET_Core Web Project

<b>Conformity Check is a project that manages legal conformity requirements for articles, suppliers and products.</b>

Conformity Check is dedicated to manage legal conformity requirements that different product types must comply with. 

In this <b>complicated regulatory world</b>, we have many <b>different product legislations, regulations, country laws and customer requirements that are necessary to be compliant with</b>.

<b>The main goal of the project is to act as a document and regulatory management system</b>.

Very common and typical case in the companies that produce <b>single or complex articles/products</b>, is to have <b>articles that need several different confirmations</b>. At the same time, these articles could have more than one supplier and the **management of the documents provided by each supplier for each article for each specific conformity type** is a **huge burden** even for the biggest companies in the World. 
In addition to the fact that **articles could be delivered for more than one source, we have separate requirements valid for different countries or client**. 

This poses the need of a web tool, that could **manage the documents, create records, relationships, generate reports, send requests and update the information provided by the suppliers**. 
Even in the current state of the art with all available software tools on the market, **the need of a system that could do all those things together and be fit to the production specifics of the today’s supply management is still high**. 
The **companies are facing more and more issues collecting, managing, evaluating and analysing the information**, subsequently – **deliver this information on request by clients, customs or control authorities around the world**. 
When we add the **inevitable fact that regulations and requirements are changing very frequently** in all the countries around the world, that the **specific restrictions are different in different countries** and the **raising awareness in human society about the environmental and people protection needs, laws and requirement**, it is **absolutely mandatory all companies to be able to manage the conformity topic with a tool that fits exactly to their needs**.

<b>This is why Conformity Check system was created for and where it could help!</b>

<b>Conformity Check is deployed online and could be found here:</b>
-	http://conformitycheck.dotnetweb.net

<b>More about the tools that Conformity Check provides and how they are realised</b>
This website is designed and runs using the main technologies below:

<b>Web Framework: </b>
-	ASP.NET Core 5

<b>Programming Languages:</b>
-	Back-end
o	C#
-	Front-end:
o	JavaScript

<b>IDE:</b> 
-	Visual Studio Enterprise 2019

<b>Database:</b>
-	MS SQL Server 2019
-	MS SQL Server Management Studio 18

<b>ORM:</b>
-	Entity Framework Core 5

<b>Markup Languages:</b>
-	HTML5 / All tables are with custom made pagination and sorting; no external libraries are used/
-	CSS /All CSS styles are custom made; no external libraries are used/

<b>External APIs:</b>
-	StyleCop Analyzers
-	SendGrid API
-	Google ReCaptcha v. 3 – for login, register and contact forms
-	Facebook authentication

<b>Additional:</b>
-	AutoMapper
-	AJAX
-	jQuery
-	All tables are with custom made pagination and sorting; no external libraries are used
-	Moq
-	xUnit
-	MockQueryable.Moq
-	Font Awesome
-	Json, CVS
-	ASP.NET CORE Areas
-	ASP.NET CORE Identity System - Scaffold identity
-	Validation of user registration e-mail - sending confirmation e-mail to user on register and on e-mail change, resend e-mail confirmation
-	Information for contact messages to admin and to contacted user - sending e-mails to both of them
-	Forgotten password confirmation with validation token on e-mail
-	Send requests to suppliers for missing conformities
-	Partial Views
-	Cookie consent
-	Validation attributes + Custom validation attributes
-	View components
-	Local Storage
-	Repository Pattern
-	Dependency Injection
-	Automatic data seeding on first application’s run
-	Code first model approach
-	Log in file - wwwroot/log/ConformityCheck.log
-	Distributed Memory Cache - SQL Server:
o	The logic is that the general view with report returned for all major entity types is generated by the order - last created one. In the cache we store with relative expiration time of 5 minutes all the records from the first page (12 records by default paging view model settings). If the user creates a new record, it will not be seen immediately, but this record will be findable be the search option available and by other sorting options except the default one! Conformity types are cached for 1 year because they are very few and rarely modified. Because the user expects to see immediately the new created ones, we update the cache after create, edit or delete of a conformity type. In the distributed cache we hold the Home/Index general counts collection for 5 minutes. In the Views we cache all view components for the general entities: all are kept for 2 minutes, just the conformity types are kept for 20 minutes since they are rarely modified. Time is relatively set.
-	Nikolay Kostov’s ASP.NET Core Template - https://github.com/NikolayIT/ASP.NET-Core-Template

<b>Database diagrams:</b>
-	Full diagram:
<img width="374" alt="DB_Diagram_All" src="https://user-images.githubusercontent.com/54845614/130635960-a55bf0bf-7159-4499-a436-8eb7fa8e3007.png">

-	All tables related to conformities:
<img width="368" alt="DB_Diagram" src="https://user-images.githubusercontent.com/54845614/130632337-281c2df0-6a99-45b2-9bcd-70aa96b9d962.png">

-	Detailed views:
<img width="621" alt="DB_Diagram1" src="https://user-images.githubusercontent.com/54845614/130632379-b4c714f4-4a6d-4215-984d-f5b420d80b9d.png">
<img width="594" alt="DB_Diagram2" src="https://user-images.githubusercontent.com/54845614/130632427-ef091388-9341-471f-9c32-03ec1df41a09.png">
<img width="610" alt="DB_Diagram3" src="https://user-images.githubusercontent.com/54845614/130632438-93076da3-2fc7-43b7-bb3c-2f14cdcebfcd.png">

<b>Pages screenshots:</b>

Home Page:

<img width="852" alt="Home" src="https://user-images.githubusercontent.com/54845614/130650266-4fbcb538-cf18-44cf-8ea3-fe248603a9fb.png">
<img width="856" alt="HomeQuickLinks" src="https://user-images.githubusercontent.com/54845614/130650278-d4f02d48-9d2a-4329-b1e3-a0749f2d07b6.png">


Register Page:

<img width="841" alt="Register" src="https://user-images.githubusercontent.com/54845614/130650374-e9c24469-6df6-4cdf-a062-44a7e66750c8.png">

Login Page:

<img width="855" alt="Login" src="https://user-images.githubusercontent.com/54845614/130650411-e4aeace7-37ea-4d8e-9730-418878ee6be5.png">


User Pages:

<img width="784" alt="User" src="https://user-images.githubusercontent.com/54845614/130652079-54d65811-7c18-4db3-9d2a-e9d37b44c210.png">
<img width="781" alt="ForgotPassword" src="https://user-images.githubusercontent.com/54845614/130650652-c03a2f89-61d2-4270-a011-5a94e44fc8ec.png">
<img width="888" alt="EmailChanged" src="https://user-images.githubusercontent.com/54845614/130651410-b023f664-bb94-4964-a1c9-972acf648cbc.png">


Articles Pages:

<img width="846" alt="Articles" src="https://user-images.githubusercontent.com/54845614/130651085-17f23a48-52ba-433b-b2f0-d629e4854e4e.png">

<img width="760" alt="LoginUserArticles" src="https://user-images.githubusercontent.com/54845614/130651703-fe47b7ee-afd3-441a-a7ce-6c395369e432.png">

<img width="760" alt="ArticleEdit" src="https://user-images.githubusercontent.com/54845614/130650973-a7dc9b14-b6c3-4d3c-b97b-6687dfb2a922.png">

<img width="762" alt="AddConformity" src="https://user-images.githubusercontent.com/54845614/130650890-074f1cea-3a49-46aa-804f-c6c4e2527618.png">

<img width="586" alt="AddConformityTypes" src="https://user-images.githubusercontent.com/54845614/130650939-875e643f-b7f5-47bc-95e5-932043470833.png">

<img width="619" alt="AddSupplier" src="https://user-images.githubusercontent.com/54845614/130650958-1eec52a3-e400-434b-a448-e1430b0c8259.png">

<img width="770" alt="ArticleEditConformities" src="https://user-images.githubusercontent.com/54845614/130651018-7e6557f4-2a84-4d03-891a-ab3b91641729.png">

<img width="775" alt="ArticleEditSupplilers" src="https://user-images.githubusercontent.com/54845614/130651056-41388f98-a826-4ab8-8189-59153d4f642e.png">

<img width="826" alt="EditConformity" src="https://user-images.githubusercontent.com/54845614/130651337-1c346fe1-f9bc-4fff-81b9-dd810d9e2817.png">

<img width="596" alt="JSSuppliersForArticlesRequiredConformities" src="https://user-images.githubusercontent.com/54845614/130651645-0cd0903e-242a-4905-ba84-205a6289c2b2.png">

<img width="602" alt="MainSupplierChange" src="https://user-images.githubusercontent.com/54845614/130651720-ed4a3af5-2b6f-43ed-a4e1-181dc14c53dd.png">

<img width="575" alt="RemoveConformitySendRequest" src="https://user-images.githubusercontent.com/54845614/130651844-0da8d10a-b488-476f-a133-9efa8783e1de.png">

<img width="588" alt="RemoveConformityType" src="https://user-images.githubusercontent.com/54845614/130651853-190b2c3a-3a73-4ca2-a683-d4c67e19c591.png">

<img width="569" alt="RemoveSupplier" src="https://user-images.githubusercontent.com/54845614/130651865-5c8d478e-9249-4545-8602-070e0c01c8a1.png">

<img width="742" alt="RequestedArticle" src="https://user-images.githubusercontent.com/54845614/130651868-6d761d6b-1caa-41bd-a4ba-348409720f1f.png">

<img width="750" alt="Requests" src="https://user-images.githubusercontent.com/54845614/130651889-625c6f63-86f2-4ca2-a44d-388b128350bb.png">

<img width="752" alt="SendRequestForSpecificArticle" src="https://user-images.githubusercontent.com/54845614/130651956-9306f2fe-6090-4a23-b7c4-f61f1be73e97.png">

<img width="838" alt="Sorting" src="https://user-images.githubusercontent.com/54845614/130652000-85ffa9bc-2816-4f41-a5eb-e6f60255e8f5.png">


Suppliers Pages:

<img width="843" alt="Suppliers" src="https://user-images.githubusercontent.com/54845614/130652019-d53ec978-29ba-42f1-afcc-9a03733d5867.png">

<img width="753" alt="SuppliersAll" src="https://user-images.githubusercontent.com/54845614/130652045-580e8e0e-507d-4851-9fe1-75ee6138efa0.png">

<img width="792" alt="QuickSearches" src="https://user-images.githubusercontent.com/54845614/130650485-fd612850-61fa-41ba-b9a9-1e884bbe873e.png">

<img width="791" alt="DetailsForSuppliers" src="https://user-images.githubusercontent.com/54845614/130651297-22774c3c-b083-4149-a2d2-fbc38be1b98e.png">

<img width="812" alt="EditSupplier" src="https://user-images.githubusercontent.com/54845614/130651350-66615b93-a024-4d27-8b6d-17a63b8b5c22.png">

<img width="774" alt="SearchResults" src="https://user-images.githubusercontent.com/54845614/130651921-722bd686-6a54-47ad-a97f-5f88310339e4.png">


Conformity Types Pages:

<img width="793" alt="ConformityTypes" src="https://user-images.githubusercontent.com/54845614/130651151-190f9af4-9fe1-4557-8fca-fd3dd9e91d54.png">


Conformity Pages:

<img width="843" alt="Conformities" src="https://user-images.githubusercontent.com/54845614/130651123-20d62197-8de5-4a24-8779-d2c327365108.png">

<img width="518" alt="CreateConformity" src="https://user-images.githubusercontent.com/54845614/130651265-4c6fbce3-51a8-4384-95cb-73d05b378da4.png">

<img width="785" alt="JSSuppliers" src="https://user-images.githubusercontent.com/54845614/130651558-f298e785-d124-42ec-9abd-36f2efd6954d.png">

<img width="412" alt="JsSuppliersArticles" src="https://user-images.githubusercontent.com/54845614/130651608-82700361-b6c5-4675-ae71-5030d31f8bd5.png">

<img width="611" alt="JSSuppliersForArticles" src="https://user-images.githubusercontent.com/54845614/130651622-ae39fc98-84f5-4b22-9ae8-77b18404ee63.png">

<img width="604" alt="Validations" src="https://user-images.githubusercontent.com/54845614/130652103-780a5e09-d213-45d1-9961-ac895e8b6e1f.png">

<img width="840" alt="ShowConformityFile" src="https://user-images.githubusercontent.com/54845614/130651979-8db4be76-065b-462d-955d-2718e0b3e39c.png">


Contact Us Pages:

<img width="825" alt="ContactUs" src="https://user-images.githubusercontent.com/54845614/130651228-cf2051df-f362-4cb5-a9c8-ab21a73a25f4.png">


Error Page:

<img width="796" alt="Error" src="https://user-images.githubusercontent.com/54845614/130651480-4cec55fd-3836-499e-b546-9ca568f560e1.png">


Emails:

<img width="654" alt="ConfirmationEmail" src="https://user-images.githubusercontent.com/54845614/130650556-a8658b96-55b2-48d5-aac1-ac3e9ecaa07a.png">

<img width="818" alt="ContactMessageToSiteAdmin" src="https://user-images.githubusercontent.com/54845614/130651218-c0b33d3c-4229-496e-9ba7-d012c5cf45fb.png">

<img width="561" alt="EmailToSupplier" src="https://user-images.githubusercontent.com/54845614/130651441-776a5077-df8d-42ec-88ad-af1406c334b6.png">

<img width="684" alt="EmailToSupplier2" src="https://user-images.githubusercontent.com/54845614/130651453-bdbc2a57-7e41-4227-a4c8-e2822837c4d0.png">

<img width="800" alt="MessageToAdmin" src="https://user-images.githubusercontent.com/54845614/130651777-8e9c7c21-708e-4a4c-b606-d2938b3e2342.png">

<img width="582" alt="ThankYouMessageFromAdmin" src="https://user-images.githubusercontent.com/54845614/130652057-2a8b4156-c38a-4c8f-94e7-2fc66bdee722.png">
