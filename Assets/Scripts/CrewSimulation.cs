﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewSimulation : MonoBehaviour
{
    [Header("References")]
    public Ship ship;
    public GameObject jobPanel;
    public GameObject combatAndManagementView;
    public GameObject mapView;
    public DistributionSystem distributionSystem; //Not needed

    [Header("Job effects")]
    [SerializeField] int sailMovementPoints = 1;
    [SerializeField] int spotterVision = 2;
    [SerializeField] float cleanHygiene = 0.1f;
    [SerializeField] float shantyLoyalty = 0.1f;
    [SerializeField] float kitchenHunger = 0.1f;
    [SerializeField] int medbayVitality = 5;
    [SerializeField] int shipWrightRepair = 5;

    [Header("Job Costs")]
    [SerializeField] int kitchenFoodConsumption = 1;
    [SerializeField] int medbayMedicineConsumption = 1;

    [Header("Character Simulation")]
    [SerializeField] float hungerReduction = 0.05f;
    [SerializeField] float hygieneReduction = 0.05f;

    public enum ShipJob { Helm, Sail, Spotter, Clean, Shanty, Kitchen, MedBay, Shipwright, Cannons, Brig, None }
    public Character[] jobs = new Character[10];
    public List<Character> charactersWithoutJobs = new List<Character>();



    public void NewTurnSimulation()
    {
        OpenSimulationWindows();
        TurnStartSimulation();
    }

    private void OpenSimulationWindows()
    {
        mapView.SetActive(false);
        combatAndManagementView.SetActive(true);
        jobPanel.SetActive(true);
    }

    public void RunJobSimulation()
    {
        SimulateJobs();

        //Muteny
    }

    private void TurnStartSimulation()
    {
        foreach (var character in ship.crew)
        {
            character.resources.Hunger -= hungerReduction;
            character.resources.Hygiene -= hygieneReduction;
        }
    }

    private void SimulateJobs()
    {
        for (int i = 0; i < jobs.Length; i++)
        {
            SimulateJob((ShipJob)i, jobs[i] != null);
        }
    }

    private void SimulateJob(ShipJob job, bool positionFilled)
    {
        switch (job)
        {
            case ShipJob.Helm:
                if (positionFilled)
                {
                    ship.remainingMovementPoints = ship.defaultMovementPoints;
                }
                else
                {
                    ship.remainingMovementPoints = 0;
                }
                break;
            case ShipJob.Sail:
                if (positionFilled && ship.remainingMovementPoints > 0)
                {
                    ship.remainingMovementPoints += sailMovementPoints;
                }
                break;
            case ShipJob.Spotter:
                ship.currentVisionRange = ship.defaultVisionRange;
                if (positionFilled)
                {
                    ship.currentVisionRange += spotterVision;
                }
                break;
            case ShipJob.Clean:
                if (positionFilled)
                {
                    foreach (var character in ship.crew)
                    {
                        character.resources.Hygiene += cleanHygiene;
                    }
                }
                break;
            case ShipJob.Shanty:
                if (positionFilled)
                {
                    foreach (var character in ship.crew)
                    {
                        character.resources.Loyalty += shantyLoyalty;
                    }
                }
                break;
            case ShipJob.Kitchen:
                if (positionFilled)
                {
                    jobs[(int)job].resources.Hunger += kitchenHunger;
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
                    jobs[(int)job].resources.Vitality += medbayVitality;
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
                    ship.Hull += shipWrightRepair;
                }
                break;
            case ShipJob.Cannons:
                break;
            case ShipJob.Brig:
                distributionSystem.Setup(ship.crew, 100, ship.crew.Count);
                break;
        }
    }

    public void SetCharacterJob(Character newCharacter, ShipJob job)
    {
        if (newCharacter.ShipJob != ShipJob.None)
        {
            RemoveCharacterFromItsJob(newCharacter);
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
            case ShipJob.Brig:
                Character oldCharacter = jobs[(int)job];
                if (oldCharacter)
                {
                    RemoveCharacterFromItsJob(oldCharacter);
                }
                jobs[(int)job] = newCharacter;
                newCharacter.ShipJob = job;
                charactersWithoutJobs.Remove(newCharacter);
                break;
        }

        Debug.Log(newCharacter.characterName + " set to job: " + job.ToString());
    }

    private void RemoveCharacterFromItsJob(Character characterToRemove)
    {
        switch (characterToRemove.ShipJob)
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
            case ShipJob.Brig:
                jobs[(int)characterToRemove.ShipJob].ShipJob = ShipJob.None;
                jobs[(int)characterToRemove.ShipJob] = null;
                charactersWithoutJobs.Add(characterToRemove);
                break;
        }

        Debug.Log(characterToRemove.characterName + " removed from it's job");
    }
}