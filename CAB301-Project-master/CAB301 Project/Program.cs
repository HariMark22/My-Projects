using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAB301_Project;

// Group 107
// Mackenzie Smith (n10233644)
// Brandon Farmer (n10628886)
// Hari Markonda Patnaikuni (n10789511)

namespace Driver
{
    class Program
    {

        Menu staffMenu;
        Menu memberMenu;
        Menu mainMenu;

        MovieCollection allMovies = new MovieCollection();
        MemberCollection allMembers = new MemberCollection(20);
        IMember currentUser;

        private Action refreshMenu; //this is used to refresh the current menu
        private Action returnMenu; //this is used to direct a user back to their previous menu

        public Program()
        {
            staffMenu = new Menu("Staff Menu");
            memberMenu = new Menu("Member Menu");
            mainMenu = new Menu("Main Menu");
        }
        //The below function InitData() initialises some movies and members and are then added to their respective collections.
        public void InitData()
        {
            IMovie movie1 = new Movie("cStar Wars1", MovieGenre.Action, MovieClassification.G, 60, 5);
            IMovie movie2 = new Movie("bStar Wars2", MovieGenre.Comedy, MovieClassification.PG, 90, 6);
            IMovie movie3 = new Movie("dStar Wars3", MovieGenre.History, MovieClassification.M, 120, 7);
            IMovie movie4 = new Movie("aStar Wars4", MovieGenre.Drama, MovieClassification.M15Plus, 150, 8);
            IMovie movie5 = new Movie("Dune", MovieGenre.Action, MovieClassification.M15Plus, 180, 20);
            IMovie movie6 = new Movie("Inception", MovieGenre.Action, MovieClassification.M, 148, 8);
            IMovie movie7 = new Movie("Avatar", MovieGenre.Action, MovieClassification.M15Plus, 162, 10);
            IMovie movie8 = new Movie("Interstellar", MovieGenre.Drama, MovieClassification.M, 169, 10);
            IMovie movie9 = new Movie("Prometheus", MovieGenre.Action, MovieClassification.M15Plus, 124, 8);
            IMovie movie10 = new Movie("Black Panther", MovieGenre.Action, MovieClassification.M15Plus, 180, 10);
            IMovie movie11 = new Movie("Avengers Infinity War", MovieGenre.Action, MovieClassification.M15Plus, 180, 10);
            IMovie movie12 = new Movie("Avengers Endgame", MovieGenre.Action, MovieClassification.M15Plus, 180, 10);

            IMember member1 = new Member("Anthony", "Starkton", "0474345778", "4321");
            IMember member2 = new Member("Hari", "Markonda", "0424749762", "1234");
            IMember member3 = new Member("Jhansi", "Markonda", "0432966678", "1234");
            IMember member4 = new Member("Mark", "Markonda", "0432966678", "1234");

            allMovies.Insert(movie1);
            allMovies.Insert(movie2);
            allMovies.Insert(movie3);
            allMovies.Insert(movie4);
            allMovies.Insert(movie5);
            allMovies.Insert(movie6);
            allMovies.Insert(movie7);
            allMovies.Insert(movie8);
            allMovies.Insert(movie9);
            allMovies.Insert(movie10);
            allMovies.Insert(movie11);
            allMovies.Insert(movie12);

            allMembers.Add(member1);
            allMembers.Add(member2);
            allMembers.Add(member3);
            allMembers.Add(member4);

        }
        public void Run()
        {
            // Generate all three UI screens consisting of mainMenu, staffMenu, and memberMenu.

            mainMenu.Add("Staff Login", AuthenticateStaff);
            mainMenu.Add("Member Login", AuthenticateUser);
            mainMenu.Add("Exit", Exit);

            staffMenu.Add("Add new DVDs of a new movie to the system", AddMovie);
            staffMenu.Add("Remove DVDs of a movie from the system", DeleteMovie);
            staffMenu.Add("Register a new member with the system", AddMember);
            staffMenu.Add("Remove a registered member from the system", RemoveMember);
            staffMenu.Add("Display a member's contact phone number, given the members name", DisplayMemberDetails);
            staffMenu.Add("Display all members who are currently renting a particular movie", DisplayBorrowers);
            staffMenu.Add("Return to the main menu", DisplayMainMenu);

            memberMenu.Add("Browse all the movies", BrowseMovies);
            memberMenu.Add("Display all the information about a movie, given the title of the movie", DisplayMovie);
            memberMenu.Add("Borrow a movie DVD", BorrowMovie);
            memberMenu.Add("Return a movie DVD", ReturnMovie);
            memberMenu.Add("List current borrowing movies", BorrowedByMember);
            memberMenu.Add("Display the top 3 movies rented by members", Top3Movies);
            memberMenu.Add("Return to the main menu", DisplayMainMenu);

            DisplayMainMenu();
        }

        //Display the details of a member in the member collection.
        //Pre-condition: Member must be in the member collection,
        //Name of member must be inputted as 'First Last'
        //Post-condition: return the members first and last name and their contact number.  
        public void DisplayMemberDetails()
        {
            refreshMenu = DisplayMemberDetails;
            IMember member;

            Console.Clear();
            Console.WriteLine("=====================User Information=====================\n");
            string memberName = UserInterface.GetInput("Please enter the name of the member you're looking for");
            string[] names = memberName.Split(' ');
            try
            {
                member = new Member(names[0], names[1]);
                if (allMembers.Search(member))
                {
                    IMember memberData = allMembers.Find(member);
                    Console.WriteLine("\n\t" + memberData.FirstName + " " + memberData.LastName + "\n\t" + memberData.ContactNumber + "\n");
                }
                else UserInterface.Error("\nUser not found in system");
            }
            catch
            {
                Console.WriteLine("\nIncorrect format. Please ensure you use the members full name like below: \n\n\t 'First Last'\n");
            }
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        //Authenticate the staff member attempting to access the staffMenu. If the Username = staff and the Password = today123 then display the staffMenu screen.
        //Pre-condition: Username must be 'staff', password must be 'today123'.
        //Post-condition: Display the staff menu screen if the staff member is authenticated.
        //If the staff member is not authenticated then return error message saying "Incorrect Staff credentials".

        public void AuthenticateStaff()
        {
            refreshMenu = AuthenticateStaff;
            Console.Clear();
            Console.WriteLine("=====================Sign In=====================\n");
            string username = UserInterface.GetInput("Username:");
            string password = UserInterface.GetPassword("Password:");
            if (username.Trim() == "staff" && password.Trim() == "today123")
            {
                DisplayStaffMenu();
            }
            else
            {
                UserInterface.Error("\nIncorrect Staff credentials");
            }
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        //Authenticate the user attempting to access the memberMenu if their first and last name and pin number are in the member collection.
        //Pre-condition: First and last name of member must be in member collection, pin must belong to the unique member.
        //Post-condition: Is the member is authenticate return message saying "Member Authenticated" and then display the member menu. 
        //If the member's name is not found in the member collection return message saying "User not found in system.
        //If the member's pin is incorrect return message saying "Password incorrect".
        public void AuthenticateUser()
        {
            refreshMenu = AuthenticateUser;
            IMember member;

            Console.Clear();
            Console.WriteLine("=====================Sign In=====================\n");
            string memberName = UserInterface.GetInput("Please enter the first and last name of member.");
            string[] names = memberName.Split(' ');
            string pin = UserInterface.GetPassword("Please enter your PIN.");
            try
            {
                member = new Member(names[0], names[1]);

                if (allMembers.Search(member))
                {
                    IMember checkMember = allMembers.Find(member);
                    if (checkMember.Pin == pin)
                    {
                        Console.WriteLine("\nMember Authenticated\n");
                        // User is current user
                        currentUser = checkMember;
                        // display member menu
                        DisplayMemberMenu();
                    }
                    else UserInterface.Error("\nPassword incorrect");
                }
                else
                {
                    UserInterface.Error("\nUser not found in system");
                }
            }
            catch
            {
                Console.WriteLine("\nIncorrect format. Please ensure you use the members full name like below: \n\n\t 'First Last'\n");
            }
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // Add a new member to the member collection if the member to be added is unique.
        //Pre-condition: First and last name of member must not contain spaces,
        //contact number must be valid, and pin must be valid.
        //Post-condition: If the member to be added meets all the preconditions add the member to the member collection and
        //return a message saying the member has successfully been added to the system.
        //If the member's first name or last name contain spaces return message saying "Ensure names don't contain spaces".
        //If the member's contact number is invalid then return the message "Incorrect contact number format".
        //If the member's pin is invalid then return the message "Incorrect pin number format".
        public void AddMember()
        {
            refreshMenu = AddMember;
            string contactNumber = "", pin = "", firstName = "", lastName = "";
            IMember member;
            Console.Clear();
            Console.WriteLine("=====================Add a Member=====================\n");
            //Might need to check if the input names are appropriate
            while (true)
            {
                firstName = UserInterface.GetInput("Please enter the members first name");
                lastName = UserInterface.GetInput("Please enter the members last name");
                member = new Member(firstName, lastName);
                if (lastName.Contains(' ') | firstName.Contains(' ')) UserInterface.Error("Ensure names don't contain spaces");
                else if (allMembers.Search(member)) UserInterface.Error("User already exists");
                else break;
            }
            while (true)
            {
                contactNumber = UserInterface.GetInput("Please enter the contact number of the member");
                if (!IMember.IsValidContactNumber(contactNumber)) UserInterface.Error("Incorrect contact number format");
                else break;
            }
            while (true)
            {
                pin = UserInterface.GetInput("Please enter the pin of the member");
                if (!IMember.IsValidPin(pin)) UserInterface.Error("Incorrect pin number format");
                else break;
            }

            member = new Member(firstName, lastName, contactNumber, pin);
            allMembers.Add(member);
            Console.WriteLine("\nMember " + member.FirstName + " " + member.LastName + " successfully added to the system!\n");
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // Given a member name, remove the member if they are in the member collection. Function also checks if member
        // has no current rentals before removing from system
        // Precondition: The first and last name must already exist in the database. First and last name must be separated by a space.
        // The member must not have any outstanding rentals.
        // Postcondition: Member is removed from the database.
        public void RemoveMember()
        {
            refreshMenu = RemoveMember;
            IMember member;
            Console.Clear();
            Console.WriteLine("=====================Remove a Member=====================\n");

            string memberName = UserInterface.GetInput("Please enter the name of the member you're looking to remove");
            string[] names = memberName.Split(' ');
            try
            {
                member = new Member(names[0], names[1]);
                if (allMembers.Search(member))
                {
                    IMember memberData = allMembers.Find(member);
                    // creating state to check if member is on any movie's borrowed list
                    string outstandingBorrows = "no";
                    foreach (Movie movie in allMovies.ToArray())
                    {
                        if (movie.Borrowers.Search(memberData))
                        {
                            UserInterface.Error("\nCannot remove. Member has outstanding borrowed movies.");
                            outstandingBorrows = "yes";
                        }
                    }
                    // if the value is unchanged, member is not on any other lists. We can safely remove from database.
                    if (outstandingBorrows == "no")
                    {
                        allMembers.Delete(memberData);
                        Console.WriteLine("\nUser " + names[0] + " " + names[1] + " has been removed from the system");
                    }
                    else
                    {
                        // resetting value back to original state for next query
                        outstandingBorrows = "no";
                    }
                                        
                }
                // Cannot find member
                else UserInterface.Error("\nUser not found in system");
            }
            // catch any malformed queries and inform user.
            catch
            {
                Console.WriteLine("\nIncorrect format. Please ensure you use the members full name like below: \n\n\t 'First Last'\n");
            }
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // When staff member inputs new movie and it's detail(e.g genre) add the new movie to the movie collection if it is unique.
        // Precondition: For a new entry, the movie must not already exist in the database. If the movie already exists, the additional
        // quanity add to existing stock.
        // Postcondition: New movie is added to the database. If the movie already exists, the available copies is incremented.
        public void AddMovie()
        {
            refreshMenu = AddMovie;
            string title = "";
            int availablecopies = 0;
            int duration = 0;
            int copies = 0;
            IMovie movie;
            Console.Clear();
            Console.WriteLine("=====================Add new DVDs or a new movie to the collection=====================\n");
            while (true)
            {
                title = UserInterface.GetInput("Please enter the movie's title");
                // Check if movie already exists in the database and inform user
                if (allMovies.Search(title) != null)
                {
                    Console.WriteLine("\nMovie already exists within system");
                    movie = allMovies.Search(title);
                    // Prompt user to enter the quanity of additional movies to add to the system.
                    while (true)
                    {
                        availablecopies = UserInterface.GetInteger("How many DVDs would you like to add to the movie");
                        if (availablecopies < 0)
                        {
                            UserInterface.Error("Number of DVDs to be added must be equal to or more than 0");
                        }
                        else break;
                    }
                    movie.AvailableCopies += availablecopies;
                    Console.WriteLine("\n" + availablecopies + " copies successfully added to " + title + "." + "\n\nNow the movie " + title + " has " + movie.AvailableCopies + " available copies.\n");
                    // Give user the option to start another entry or return to the previous menu
                    ReturnTo(refreshMenu, returnMenu);
                }
                // Catch if nothing was entered in the first place
                else if (string.IsNullOrWhiteSpace(title))
                {
                    UserInterface.Error("\nMovie title entered is empty");
                }
                else break;
            }
            // The movie must be new. Take the rest of the necessary information from the staff and enter into the database.
            while (true)
            {
                Console.WriteLine("\nWhat is the genre of the movie? Enter one of the below numbers corrosponding to the movie's genre.\n1. Action\n2. Comedy\n3. History\n4. Drama\n5. Western\n");
                int genre1 = UserInterface.getOption(1, 5) + 1;
                MovieGenre genre = (MovieGenre)Convert.ToInt32(genre1);
                Console.WriteLine("You entered " + genre1 + ". " + genre);

                Console.WriteLine("\nWhat is the classfication of the movie? Enter one of the below numbers corrosponding to the movie's classfication.\n1. G\n2. PG\n3. M\n4. M15Plus\n");
                int classification1 = UserInterface.getOption(1, 4) + 1;
                MovieClassification classification = (MovieClassification)Convert.ToInt32(classification1);
                Console.WriteLine("You entered " + classification1 + ". " + classification);
                
                while (true)
                {
                    duration = UserInterface.GetInteger("\nPlease enter the movie's duration in minutes");
                    if (duration < 0)
                    {
                        UserInterface.Error("Ensure duration in minutes is positive");
                    }
                    else break;

                }
                while (true)
                {
                    copies = UserInterface.GetInteger("\nPlease enter the number of available copies of the movie");
                    if (copies < 0)
                    {
                        UserInterface.Error("Ensure number of available copies is positive");
                    }
                    else break;
                }
                allMovies.Insert(new Movie(title, genre, classification, duration, copies));
                Console.WriteLine("\nSuccessfully added " + title + " to the movie collection!\n");
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
        }

        // Delete a movie from the movie collection if the movie is in the collection.
        // Precondition: movie already exists in the database, the available copies should be zero, and the movie must not be out on loan.
        // movie title will be entered as a string
        // Postcondition: Movie is deleted from the database.
        public void DeleteMovie()
        {
            refreshMenu = DeleteMovie;
            Console.Clear();
            Console.WriteLine("=====================Delete DVDs from the movie collection=====================\n");
            string Title = UserInterface.GetInput("Please enter the movie's title");
            if (allMovies.Search(Title) == null)
            {
                Console.WriteLine("\nMovie not in the collection.\n");
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
            else
            {
                // Retrieve the details of the movie from the database and get the current amount in the database.
                IMovie movie = allMovies.Search(Title);
                int x = movie.Borrowers.Number;
                int removecopies = 0;
                if (movie.AvailableCopies > 0)
                {
                    Console.WriteLine("\nMovie found in movie collection");
                    // Get the amount of movies the staff wishes to delete and the amount > 0.
                    while (true)
                    {
                        removecopies += UserInterface.GetInteger("How many DVDs would you like to remove?");
                        if (removecopies < 0)
                        {
                            UserInterface.Error("Number of DVDs to be removed must be more than 0");
                        }
                        else break;
                    }
                    movie.AvailableCopies -= removecopies;

                    // If more than the amount in the database is selected, reduce to zero
                    if (movie.AvailableCopies < 0)
                    {
                        movie.AvailableCopies = 0;
                    }
                    Console.WriteLine("\nDVDs successfully removed from " + Title + "." + "\n\nNow the movie " + Title + " has " + movie.AvailableCopies + " available copies.");

                }
                // If there is no inventory and its not being borrowed, delete the movie out of the database.
                if (movie.AvailableCopies == 0 & x == 0)
                {
                    allMovies.Delete(new Movie(Title));
                    Console.WriteLine("\nMovie successfully removed from movie collection since no one is borrowing the movie.\n");
                }
                // If there are copies being borrowed, inform the staff that the movie has not been deleted out of the system.
                if (movie.AvailableCopies == 0 & x != 0)
                {
                    Console.WriteLine("Movie has 0 available copies and is being borrowed by a borrower. Hence, it will not be removed from the movie collection. ");
                }
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
        }

        // Display the details of a movie if it is in the movie collection
        // Precondition: Movie must already exists in the database.
        // Postcondition: If the movie exists, the title, genre, classification, duration, and the number of available copies is displayed.
        // If the movie does not exist, user is notified.
        public void DisplayMovie()
        {
            refreshMenu = DisplayMovie;
            IMovie movie;

            Console.Clear();
            Console.WriteLine("=====================Movie Information=====================\n");
            string movieTitle = UserInterface.GetInput("Please enter the title of the movie you're looking for");
            movie = allMovies.Search(movieTitle);
            if (movie != null) Console.WriteLine("\n" + movie.ToString());
            else UserInterface.Error("\nMovie not found in system");
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // Return as a string all the movies in the movie collection.
        // Precondition: there must be at least one movie in the collection to display.
        // Postcondition: Movies are displayed to the user. If no movies, exists, the user is notified.
        public void BrowseMovies()
        {
            refreshMenu = BrowseMovies;
            int x = 0;

            Console.Clear();
            Console.WriteLine("=====================Browsing all Movies=====================\n");
            foreach (Movie movie in allMovies.ToArray())
            {
                x++;
                Console.WriteLine(movie.ToString() + "\n");
            }
            if (x == 0)
            {
                Console.WriteLine("No movies are in the movie collection.\n");

            }
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // Let a member borrow a movie if it is in the movie collection and copies to borrow are available.
        // Precondition: The movie already exists in the system and there is at least 1 available copy. Member cannot rent
        // a movie more than once.
        // Postcondition: The number of available copies decreases by 1 and member is added to the movie's list of borrowers.
        public void BorrowMovie()
        {
            refreshMenu = BorrowMovie;
            Console.Clear();
            Console.WriteLine("=====================Borrow Movie=====================\n");
            string query = UserInterface.GetInput("Please enter the movie you would like to borrow");

            if (allMovies.Search(query) == null)
            {
                Console.WriteLine("\nMovie intended to borrow is not in the movie collection.\n");
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
            else
            {
                IMovie movie = allMovies.Search(query);
                int x = movie.Borrowers.Number;
                if (!movie.Borrowers.Search(currentUser) & movie.AvailableCopies != 0 & x < 10)
                {
                    movie.AddBorrower(currentUser);
                    Console.WriteLine("\n" + query + " successfully borrowed.\n");
                }
                else
                {
                    Console.WriteLine("\nYou are either already borrowing this movie or there are no more copies of this movie available to borrow.\nHence, you cannot borrow another copy.");
                }
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }

        }

        // Let a member return a movie if the movie is in the collection
        // Precondition: The title, inputted as a string, exists in the database. The database must also have a record of the borrow taking place.
        // Postcondition: The number of available movies increments by 1. Logged in member is removed from the movie's borrow list.
        public void ReturnMovie()
        {
            refreshMenu = ReturnMovie;
            Console.Clear();
            Console.WriteLine("=====================Return Movie=====================\n");
            string query = UserInterface.GetInput("Please enter the movie you would like to return.");

            if (allMovies.Search(query) == null)
            {
                Console.WriteLine("\nThere are no records of this rental. Please select an option to try again.\n");
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
            else
            {
                IMovie movie = allMovies.Search(query);
                if (movie.Borrowers.Search(currentUser))
                {
                    movie.RemoveBorrower(currentUser);
                    Console.WriteLine("\n" + query + " successfully returned.\n");
                }
                else
                {
                    Console.WriteLine("\nNo record of current user renting movie or possibly misspelled Cannot return.\n");
                }
                // Give user the option to start another entry or return to the previous menu
                ReturnTo(refreshMenu, returnMenu);
            }
        }

        // Allow the staff to view the borrowers of a given movie
        // Precondition: The movie, given as a string, exists in the database.
        // Postcondition: The number of borrowers and a list of those borrowers, is displayed to the staff.
        public void DisplayBorrowers()
        {
            refreshMenu = DisplayBorrowers;
            IMovie movie;

            Console.Clear();
            Console.WriteLine("=====================Movie Borrowers=====================\n");
            string movieTitle = UserInterface.GetInput("Please enter the title of the movie you're looking for");
            movie = allMovies.Search(movieTitle);
            if (movie != null)
            {
                Console.WriteLine("\nThis movie currently has " + movie.Borrowers.Number + " borrowers.\n");
                if (movie.Borrowers.Number > 0) Console.WriteLine("They are: \n" + movie.Borrowers.ToString());
            }
            else UserInterface.Error("\nMovie not found in system");
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // Display to the user the current list of movies borrowed from the database.
        // Precondition: none
        // Postcondition: A list, if any, of movies that the user is currently renting.
        public void BorrowedByMember()
        {
            refreshMenu = BorrowedByMember;
            Console.Clear();
            Console.WriteLine("=====================Borrowed Movies=====================\n");
            Console.WriteLine("Here are the movies you are currently borrowing:\n");
            foreach (Movie movie in allMovies.ToArray())
            {
                if (movie.Borrowers.Search(currentUser)) Console.WriteLine(movie.Title);
            }
            Console.WriteLine("\n");
            // Give user the option to start another entry or return to the previous menu
            ReturnTo(refreshMenu, returnMenu);
        }

        // After an action is performed within a menu, display options to the user on whether to refresh the same menu or return to the 
        // previous menu.
        // Precondition: none
        // Postcondition: Current user is taken to the previous menu or the current menu is refreshed.
        public void ReturnTo(Action refreshMenu, Action returnMenu)
        {
            int check;
            Console.WriteLine("Enter your choice ==> (1/2)");
            Console.WriteLine("  1. Refresh");
            Console.WriteLine("  2. Return to previous menu");
            check = UserInterface.getOption(1, 2);
            if (check + 1 == 1) refreshMenu();
            else returnMenu();
        }

        // Display the Main Menu of the application
        // Precondition: nil
        // Postcondition: User is displayed the Main Menu of the application.
        public void DisplayMainMenu()
        {
            returnMenu = DisplayMainMenu;
            Console.Clear();
            mainMenu.Display();
        }
        // Display the Staff Menu of the application
        // Precondition: nil
        // Postcondition: User is displayed the Staff Menu of the application.
        public void DisplayStaffMenu()
        {
            returnMenu = DisplayStaffMenu;
            Console.Clear();
            staffMenu.Display();
        }
        // Display the Member Menu of the application
        // Precondition: nil
        // Postcondition: User is displayed the Member Menu of the application.
        public void DisplayMemberMenu()
        {
            returnMenu = DisplayMemberMenu;
            Console.Clear();
            memberMenu.Display();
        }
        // Exit the Console application
        // Precondition: nil
        // Postcondition: Application terminated.
        public void Exit()
        {
            Environment.Exit(0);
        }

        // Display the top 3 borrowed movies of the database to the user.
        // Precondition: There is at least one movie that has been borrowed from the database.
        // Postcondition: A list of, at most, 3 of the most borrowed movies is displayed to the user.
        public void Top3Movies()
        {
            refreshMenu = Top3Movies;
            IMovie[] movieArray = allMovies.ToArray();
            Console.Clear();
            Console.WriteLine("=====================Top 3 Movies=====================\n");
            int n = movieArray.Length;

            if (n < 1)
            {
                Console.WriteLine("\nNo movies in collection\n\n");
                ReturnTo(refreshMenu, returnMenu);
            }

            IMovie[] topThreeMovies = GetTop3(movieArray);

            int n2 = n < 3 ? n : 3;
            for (int i = 0; i < n2; i++)
            {
                if (topThreeMovies[i].NoBorrowings < 1) break;
                Console.WriteLine(i + 1 + ": " + topThreeMovies[i].Title +
                        ", with " + topThreeMovies[i].NoBorrowings + " borrows");

            }
            if (topThreeMovies[0].NoBorrowings == 0) Console.WriteLine("No movies have been borrowed");
            Console.WriteLine("");
            ReturnTo(refreshMenu, returnMenu);
        }

        // Search the database for the top 3 borrowed movies
        // Precondition: Movies have been borrowed from the database.
        // Postcondition: Returns a Movie array of the top 3 movies
        private static IMovie[] GetTop3(IMovie[] movies)
        {
            IMovie first, second, third;
            first = second = third = new Movie("", MovieGenre.Action, MovieClassification.G, 60, 5);

            for (int i = 0; i < movies.Length; i++)
            {
                if(movies[i].NoBorrowings > first.NoBorrowings)
                {
                    third = second;
                    second = first;
                    first = movies[i];
                }
                else if (movies[i].NoBorrowings > second.NoBorrowings)
                {
                    third = second;
                    second = movies[i];
                }
                else if (movies[i].NoBorrowings >= third.NoBorrowings)
                {
                    third = movies[i];
                }
            } 
            return (new IMovie[] {first, second, third});
        }

        // Initialize data if applicable and starting application
        static void Main(string[] args)
        {
            Program menu = new Program();
            //data used for testing
            //menu.InitData();
            menu.Run();
        }
    }

}
