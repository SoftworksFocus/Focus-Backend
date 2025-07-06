<p align="center">
  <a href="https://github.com/NicolasLS-Tech/Primeiro-Rep" target="blank"><img src="https://github.com/NicolasLS-Tech/Primeiro-Rep/blob/main/FocusLogo.jpg" width="350" alt="Focus Logo" /></a>
</p>

<h4 align="center">Organization is the Focus</h4>

# Description

**Focus** is a public application designed to facilitate the efficient publication, organization and execution of general activities. Its primary objective is to facilitate connections between individuals who share common interests, such as fitness, academic achievements, and other relevant topics.

# Features

* Possibility to **create your own group** for general purpose

* **Connect** to other people with the same interests as yours.

* **Simple, minimalistic, and complete software**

* Attach **files** and make your little task a great achievement.

# Installation/Use

**Focus** has two platforms that will allow its operation. Firstly, it is necessary to have a personal computer or a browser (or a mobile browser with computer mode enabled). Access the **Focus** website using this link via your preferred web browser.
You can download our mobile app by clicking here.

## Backend Setup Tutorial

To start using the **Focus** system backend on your computer, you must ensure you have the following applications installed:
***
* **Docker Desktop/Terminal**
* **PostgreSQL**
* **Web Browser**
***
First, you need to have the **[Postgres](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)** image in Docker. Once that is done, the next step is to create a database container based on this image. The main way to do this is via the terminal, using the command:
```
docker run --name yourContainerName -e POSTGRES_PASSWORD=YourPassword -e POSTGRES_DB=yourDbName -p 5432:5432 -d postgres
```
Alternatively, using Docker Desktop, you can simply run ("play") the **[Postgres](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)** image (previously installed in the application) and set the environment variables through the graphical interface.

After that, using your preferred IDE (Integrated Development Environment), connect to the database/container created in the previous step. Use the port, user (the default is "postgres"), and the password you defined. For detailed guides, follow the links for **[Rider](https://www.jetbrains.com/help/rider/Connecting_to_a_database.html)** or **[Visual Studio](https://learn.microsoft.com/en-us/visualstudio/data-tools/add-new-connections?view=vs-2022)**.

Only then can the tests be run on the application. By using the **"Swagger"** tool, which is launched automatically with the project, the user will be able to test the **API (Application Programming Interface)** and ensure that the **Focus** system backend is running correctly on their machine.
***
# Contact

Should you have any queries regarding the system, or wish to report a bug, please direct them to the following email address: focusteam@focus.com.
