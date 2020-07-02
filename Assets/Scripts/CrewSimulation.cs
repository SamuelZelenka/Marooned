using System.Collections.Generic;
using UnityEngine;

public class CrewSimulation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject jobPanel = null;

    [Header("Job effects")]
    [SerializeField] int sailMovementPoints = 1;
    [SerializeField] int spotterVision = 2;
    [SerializeField] int cleanHygiene = 10;
    [SerializeField] int shantyLoyalty = 10;
    [SerializeField] int kitchenHunger = 10;
    [SerializeField] int medbayVitality = 5;
    [SerializeField] int shipWrightRepair = 5;

    [Header("Job Costs")]
    //[SerializeField] int kitchenFoodConsumption = 1;
    //[SerializeField] int medbayMedicineConsumption = 1;

    [Header("Character Simulation")]
    [SerializeField] int hungerReduction = 5;
    [SerializeField] int hygieneReduction = 5;

    public enum ShipJob { Helm, Sail, Spotter, Clean, Shanty, Kitchen, MedBay, Shipwright, Cannons, None }
    [SerializeField] JobPosition[] jobs = new JobPosition[9];

    public void NewTurnSimulation()
    {
        OpenSimulationWindows();
        TurnStartSimulation();
    }

    public void OpenSimulationWindows()
    {
        jobPanel.SetActive(true);
    }


    private void TurnStartSimulation()
    {
        HexGridController.player.Ship.remainingMovementPoints = 0;
        foreach (var character in HexGridController.player.Crew)
        {
            character.characterData.Hunger.CurrentValue -= hungerReduction;
            character.characterData.Hygiene.CurrentValue -= hygieneReduction;
        }
    }

    public void ConfirmJobPositions()
    {
        jobPanel.SetActive(false);
        SimulateJobs();


        //Muteny

        HexGridController.player.Ship.StartNewTurn();
    }


    private void SimulateJobs()
    {
        for (int i = 0; i < jobs.Length; i++)
        {
            SimulateJob((ShipJob)i, jobs[i].HasCharacter);
        }
    }

    private void SimulateJob(ShipJob job, bool positionFilled)
    {
        Ship playerShip = HexGridController.player.Ship;

        switch (job)
        {
            case ShipJob.Helm:
                if (positionFilled)
                {
                    playerShip.remainingMovementPoints = playerShip.defaultMovementPoints;
                }
                else
                {
                    playerShip.remainingMovementPoints = 0;
                }
                break;
            case ShipJob.Sail:
                if (positionFilled && playerShip.remainingMovementPoints > 0)
                {
                    playerShip.remainingMovementPoints += sailMovementPoints;
                }
                break;
            case ShipJob.Spotter:
                playerShip.currentVisionRange = playerShip.defaultVisionRange;
                if (positionFilled)
                {
                    playerShip.currentVisionRange += spotterVision;
                }
                break;
            case ShipJob.Clean:
                if (positionFilled)
                {
                    foreach (var character in HexGridController.player.Crew)
                    {
                        character.characterData.Hygiene.CurrentValue += cleanHygiene;
                    }
                }
                break;
            case ShipJob.Shanty:
                if (positionFilled)
                {
                    foreach (var character in HexGridController.player.Crew)
                    {
                        character.characterData.Loyalty.CurrentValue += shantyLoyalty;
                    }
                }
                break;
            case ShipJob.Kitchen:
                if (positionFilled)
                {
                    jobs[(int)job].characterOnJob.characterData.Hunger.CurrentValue += kitchenHunger;
                    //Consume raw food
                }
                //Open Split window
                //if (positionFilled)
                //{
                //    distributionSystem.Setup(ship.crew, 100, ship.crew.Count, CharacterResources.ResourceType.Hunger);
                //}
                break;
            case ShipJob.MedBay:
                if (positionFilled)
                {
                    jobs[(int)job].characterOnJob.characterData.Vitality.CurrentValue += medbayVitality;
                    //Consume medicine
                }
                //Open Split window
                //if (positionFilled)
                //{
                //    distributionSystem.Setup(ship.crew, 100, ship.crew.Count, CharacterResources.ResourceType.Vitality);
                //}
                break;
            case ShipJob.Shipwright:
                if (positionFilled)
                {
                    playerShip.Hull += shipWrightRepair;
                }
                break;
            case ShipJob.Cannons:
                break;
        }
    }

    public void SetCharacterJob(Character newCharacter, ShipJob job)
    {
        if (newCharacter.characterData.ShipJob != ShipJob.None)
        {
            RemoveCharacterFromItsJob(newCharacter, job);
        }

        switch (job)
        {
            case ShipJob.Helm:
            case ShipJob.Sail:
            case ShipJob.Spotter:
            case ShipJob.Clean:
            case ShipJob.Shanty:
            case ShipJob.Kitchen:
            case ShipJob.MedBay:
            case ShipJob.Shipwright:
            case ShipJob.Cannons:
                Character oldCharacter = jobs[(int)job].characterOnJob;
                if (oldCharacter)
                {
                    RemoveCharacterFromItsJob(oldCharacter, job);
                }
                jobs[(int)job].SetCharacterToJob(newCharacter);
                newCharacter.characterData.ShipJob = job;
                break;
        }

        Debug.Log(newCharacter.characterData.characterName + " set to job: " + job.ToString());
    }

    private void RemoveCharacterFromItsJob(Character characterToRemove, ShipJob job)
    {
        switch (job)
        {
            case ShipJob.Helm:
            case ShipJob.Sail:
            case ShipJob.Spotter:
            case ShipJob.Clean:
            case ShipJob.Shanty:
            case ShipJob.Kitchen:
            case ShipJob.MedBay:
            case ShipJob.Shipwright:
            case ShipJob.Cannons:
                jobs[(int)job].RemoveCharacter();
                characterToRemove.characterData.ShipJob = ShipJob.None;
                break;
        }

        Debug.Log(characterToRemove.characterData.characterName + " removed from it's job");
    }
}