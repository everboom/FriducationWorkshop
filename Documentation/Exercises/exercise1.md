# Friducation DevOps Workshop

1. Browse to www.hotmail.com and create a new hotmail account. 
   Don't forget to store the username and password for later use.
2. Browse to dev.azure.com and click the button that says Start Free.
   Login with your new hotmail account.
3. Follow the prompt on the screen to create an organization and a project.
   Name them whatever you like. Make sure it is a Private project. 

Now that we have a project in Azure DevOps, we need to import the code for our website.
1. Click on the button that says "import repository".
2. In the 'Clone Url' field, copy paste the following url:
   https://github.com/everboom/FriducationWorkshop.git
   This will copy the code from the demo website into our project.
   *Note: You can leave the option "Requires Authentication" unchecked.*

Now we need to automate the build process of this project and put in some tests.
1. On the menu in the left, go to Azure Pipelines. The top item in the menu is also called Pipelines, make sure you are on that page.
2. Press the button 'Create Pipeline' to start creating a new pipeline. 
3. At the bottom of the page, click the "Use the classic editor" link.
4. Make sure 'Azure Repos Git' is selected as your top option. It should also automatically select the project and repository we've created during these exercises.
5. Press 'Continue'.
6. At the top of the page, select "start with an empty job'.
7. On the bar that says "Agent Job 1", Press the + button to add a new Task. 
8. Select the .NET Core task in the Marketplace.
9. Add the same task 2 more times. 
10. Select the first task you added and set the 'Command' option to 'restore'.
11. Select the second task you added and set the 'Command' option to 'build'.
12. Select the third task you added and set the 'Command' option to 'test'.

We've constructed a pipeline to automatically download the dependencies for the project, then build the code and turn it into an application and finally, to run automated tests on them.

Let's see if the pipeline succeeds!

1. Press the Save and Queue or the Save and then the Queue button to start the pipeline.
2. :warning: The build will have failed with the following error: 
   No hosted parallelism has been purchased or granted.

What does this mean? It means we do not currently have an agent available to run the pipeline. We could request free resource from Microsoft, but this will take a while. We could also pay for the agents, but we probably want to just play around for now. The easiest and cheapest option then is to set up our own pipeline agent, so let's do that now (with the help of the teacher).

The first thing we'll need is a 'Personal Access Token'. This is a way for the agent application to show to Azure DevOps that it is authorized to connect to our DevOps organization.

1. Find the User Settings button in the top right of the DevOps screen, next to your user information. It looks like a person with a little cogwheel next to it. Click on it to open the User Settings menu. Near the bottom of the menu, find the option 'personal access tokens' and click it.
2. On the next page, press the button that says '+ New Token'.
3. Under Name, choose a name for your token, like 'agenttoken'.
4. At the bottom of the new panel, find the link that says 'Show all scopes' and click it.
5. Now at the top of the list, find the item 'Agent Pools'. Check the checkbox that says 'Read & Manage'.
6. At the bottom of the panel, Press the 'Create' button.
7. On the next page, copy paste the code and store it in a text file. This is the Personal Access Token we'll need to create an agent. After this, press the 'Close' button to go back to the main Azure DevOps window.
8. Now copy paste the URL from your browser into the same text file. 
9. :warning: Now copy paste both of the values to your teacher in Microsoft Teams, and he will create a new Self-Hosted agent for you to use.

We must now configure our pipeline to make use of the new agent. 

1. Navigate back to the settings of your newly created pipeline so we can edit it.
2. Click on the top bar, that says 'Pipeline'. Then change the setting for Agent pool from 'Azure Pipelines' to 'Default'.
3. At the top of the page, click on 'Save and Queue'. You will be redirected to a page where you can see if the pipeline runs successfully. 

The pipeline should have run correctly this time. You can see the results in the pipeline run page. You can see the log messages of the restore, build and test phases. You can also find a detailed report for the Unit tests.

We now need to create a Release Pipeline to publish our web application to Azure so our customers can visit it.

1. Go back to the pipeline editing page and add another .NET Core task. This one needs 'Publish' for its 'command' setting.
2. In the new task, copy paste the following text into the 'Arguments' field.
   ```--configuration Release --output $(Build.ArtifactStagingDirectory)```
3. Finally, add a new task called 'Publish build artifacts'. You can find this task quickly by typing its name into the search bar. 
4. Press the 'Save and Queue' button.

This will ensure that the pipeline produces a build artifact which we can publish to Azure and put our website online. Let's now create a release pipeline which will do this for us.

1. In the project menu, click on Pipelines, then Releases.
2. Click the 'New Pipeline' button.
3. Under 'Select a template', click 'Empty job'.

First, Now we must ensure the release pipeline has access to the original repository, because it contains a template that we will use to create our web application in Azure.
1. Click on 'Add an Artifact'.
2. In the 'Source type' field, select 'Azure Repos Git'.
3. For 'Project', select our project.
4. For 'Source (repository)', select its Repository.
5. For 'Default branch', select 'main'.
6. Click 'Add' to add the artifact. 

4. Create a variable with the name 'WebAppName'. Then think of a name for your new web application. This will form part of the url for your website later. For instance: friducation-[yourname]

5. Rename the Pipeline stage to 'Resource Deployment'.
6. Click on the Agent Job and set it to use the 'Default' agent pool. 

We must now add a task that will create a new web application in Azure, but to do this we must authorize Azure DevOps to do some work in Azure. Normally you would link Azure DevOps to your own Azure tenant during this step but for this exercise this is simplified by using an existing Azure Subscription managed by the teacher.

1. Click Project Settings in the bottom left, then navigate to 'Service Connections' under the 'Pipelines' section.
2. Click 'Create Service Connection'
3. Select 'Azure Resource Manager' and click on 'Next'.
4. Then select 'Service Principal (manual)' and click 'Next'.
5. For the 'Subscription Id' field, set value to: ```f05495e0-9123-402d-89b6-0c8f63339b73```
6. For the 'Subscription Name' field, set value to: ```DevOpsWorkshops```
7. For the 'Tenant Id', set value to: ```d9e835f3-6333-4986-ba37-74778153ebc5```
8. For the 'Service Principal Id', set value to ```7aa0a19b-d7b9-40ff-a2d0-951df955518b```
9. For the 'Service Principal Key', set value to: ```yUr8Q~MODnppP1e36gzHZnZsF0RjNfP6Mpm90dbg```
10. Click 'Verify' to ensure you've set the values correctly.
11. For 'Service Connection name', set value to: ```Azure Connection```
12. Check the box that says 'Grant access permission to all pipelines'.
13. Save the Service Connection and navigate back to the Release Pipeline we created in an earlier step.

We can now finish setting up the release pipeline. It needs two tasks: one to create a web application in Azure, and one to publish our build artifact to it.

1. Start editing the previously created Release Pipeline.
2. Navigate to tasks.
3. Use the '+' button to add an ARM Template Deployment task.
4. Click on the new task to edit it.
5. In the dropdown menu for 'Azure Resource Manager' select our new Service Connection.
6. In the Resource Group field, type: $(WebAppName)
7. In the Location field, select 'West Europe'
8. Under the Template field, select the Ellipses button (three dots). 
9. In the new window, navigate to the item that ends in (Azure Repos Git), then to the folder Templates, then select webapp.bicep. Then press 'Ok'.
10. The value under Template should look like this: ```$(System.DefaultWorkingDirectory)/[Project Name]/Templates/webapp.bicep```
11. Under 'Override template parameters', copy paste: ```-webAppName $(WebAppName)```
12. Save the Release Pipeline.

If you run the release pipeline now, it should create a new web app in Azure that we can then publish to. You can go ahead and run the release pipeline now if you wish. While it's running, we can go back and add the final step to the pipeline.

First, we must ensure that the release pipeline knows how to find the build artifact from the previous pipeline. 
1. Navigate to the Release Pipeline editing page. On the left under Artifacts, click ```+ Add``` to add a source artifact to the Release Pipeline. 
2. In the 'Source type' field, make sure 'Build' is selected.
3. Select the Build pipeline we created earlier in the exercise.

1. Navigate to the Tasks editing page of your release pipeline and add a task to the Agent Job: 'Azure App Service Deploy'
2. Under 'Azure Subscription', select the Service Connection we created earlier, which should be called 'Azure Connection'.
3. Under App Service Name, type ```$(WebAppName)```
4. Save the pipeline and press the button 'Create Release'.

Once the release finishes running, you should be able to visit your website at:
*https://[yourname].azurewebsites.net*
Where [yourname] is the value you chose for the variable ```$WebAppName```.

