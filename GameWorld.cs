using System;
using System.Transactions;

namespace StarterGame
{
    public  class GameWorld
    {	
	    private static GameWorld _instance = null;
	    public  static GameWorld Instance
	    {
		    get
		    {
			    if(_instance == null)
			    {
				    _instance = new GameWorld();
			    }
			    return _instance;
		    }
	    }

        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        private Room _exit;
        public Room Exit { get { return _exit; } }
        private int counter;

        private Room _trigger;
        private Room _sideA;
        private Room _sideB;
        private string _toSideA;
        private string _toSideB;

        private GameWorld()
        {
            CreateWorld();
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
            NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
                counter = 0;
        }

        public void PlayerDidEnterRoom(Notification notification)
            {
                Player player = (Player)notification.Object;
                if(player!= null)
                {
                    if(player.CurrentRoom == Exit)
                    {
                        player.OutputMessage("\n*** the player reacher the exit ");
                        counter++;
                        if (counter == 5)
                        {
                            Exit.SetExit("shortcut", Entrance);
                            Entrance.SetExit("shortcut", Exit);
                        }

                    }
                    if(player.CurrentRoom == Entrance)
                    {
                        player.OutputMessage("\n *** Ther play came back to the entrance.");                    
                    }
                    if(player.CurrentRoom == _trigger)
                    {
                        _sideA.SetExit(_toSideB, _sideB);
                        _sideB.SetExit(_toSideA, _sideA);
                        player.OutputMessage("There is a change in the world.");

                    }
                }
            }


            public void PlayerWillEnterRoom(Notification notification)
            {
                Player player = (Player)notification.Object;
                if (player != null)
                {
                    if(player.CurrentRoom == Entrance)
                    {
                        player.OutputMessage("\n>>> the player is leaving the entrance");
                    }
                    if(player.CurrentRoom == Exit)
                    {
                        player.OutputMessage("\n >>> The player is going away from the exit. ");
                    }
                }
            }

            private void CreateWorld()
            {
                Room outside = new Room("outside the main entrance of the university");
                Room scctparking = new Room("in the parking lot at SCCT");
                Room boulevard = new Room("on the boulevard");
                Room universityParking = new Room("in the parking lot at University Hall");
                Room parkingDeck = new Room("in the parking deck");
                Room scct = new Room("in the SCCT building");
                Room theGreen = new Room("in the green in from of Schuster Center");
                Room universityHall = new Room("in University Hall");
                Room schuster = new Room("in the Schuster Center");

               //
                outside.SetExit("west", boulevard);

                boulevard.SetExit("east", outside);
                boulevard.SetExit("south", scctparking);
                boulevard.SetExit("west", theGreen);
                boulevard.SetExit("north", universityParking);

                scctparking.SetExit("west", scct);
                scctparking.SetExit("north", boulevard);

                scct.SetExit("east", scctparking);
                scct.SetExit("north", schuster);

                schuster.SetExit("south", scct);
                schuster.SetExit("north", universityHall);
                schuster.SetExit("east", theGreen);

                theGreen.SetExit("west", schuster);
                theGreen.SetExit("east", boulevard);

                universityHall.SetExit("south", schuster);
                universityHall.SetExit("east", universityParking);

                universityParking.SetExit("south", boulevard);
                universityParking.SetExit("west", universityHall);
                universityParking.SetExit("north", parkingDeck);

                parkingDeck.SetExit("south", universityParking);

                //Extra rooms
                Room davidson = new Room("In the davidson center");
                Room clockTower = new Room("at the clock tower");
                Room greekCenter = new Room("at the greek center.");
                Room woodall = new Room("at woodall hall0");

                //Connect the special rooms
                davidson.SetExit("west", clockTower);

                clockTower.SetExit("north", greekCenter);
                clockTower.SetExit("south", woodall);
                clockTower.SetExit("east", davidson);

                greekCenter.SetExit("south", clockTower);

                woodall.SetExit("north", clockTower);

                // Setup Connection
                _trigger = parkingDeck;
                _sideA = schuster;
                _sideB = davidson;
                _toSideA = "east";
                _toSideB = "west";



                // Assign special rooms
                _entrance = outside;
                _exit = schuster;

               // return outside;
            }

    }

}


