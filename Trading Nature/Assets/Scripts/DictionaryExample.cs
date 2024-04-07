using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryExample : MonoBehaviour
{
    public Text display;
    public Text display01;
    public Text display02;
    public Text display03;
    public Text display04;

    public void Start()
    {
        NatureGained["RAWLEAF"] = 0;
        NatureGained["RAWFLOWER"] = 0;
        NatureGained["RAWSTREAM"] = 0;
        NatureGained["RAWSUNLIGHT"] = 0;
    }

    public void Update()
    {
        // Each update, display the resources available to the player
        DisplayResources();
        // And what items the player already has.
        DisplayItems();
        
        NatureGained["RAWLEAF"] += 2f * Time.deltaTime;
        NatureGained["RAWFLOWER"] += 0.5f * Time.deltaTime;
        NatureGained["RAWSTREAM"] += 2f * Time.deltaTime;
        NatureGained["RAWSUNLIGHT"] += 0.1f * Time.deltaTime;

        display01.text = NatureGained["RAWLEAF"].ToString("F2");
        display02.text = NatureGained["RAWFLOWER"].ToString("F2");
        display03.text = NatureGained["RAWSTREAM"].ToString("F2");
        display04.text = NatureGained["RAWSUNLIGHT"].ToString("F2");

    }

    // A dictionary to represent what resources they have.
    private Dictionary<string, int> resourcesOwned = new Dictionary<string, int>();
    // A dictionary to represent what items they have.
    private Dictionary<string, int> itemsOwned = new Dictionary<string, int>();
    
    private Dictionary<string, float> NatureGained = new Dictionary<string, float>();
    

    // This function adds a resource.
    public void AddResource(string resourceType, int amountToAdd)
    {
        if (resourcesOwned.ContainsKey(resourceType))
        {
            resourcesOwned[resourceType] = resourcesOwned[resourceType] + amountToAdd;
            
            Debug.Log("You own " + resourcesOwned[resourceType] + " of " + resourceType);
        }
        else
        {
            resourcesOwned.Add(resourceType, amountToAdd);
        }
    }

    // This function removes a resource
    public bool RemoveResource(string resourceType, int cost)
    {
        if (!HasEnoughResources(resourceType, cost))
        {
            return false;
        }

        resourcesOwned[resourceType] = resourcesOwned[resourceType] - cost;

        if (resourcesOwned[resourceType] == 0)
            resourcesOwned.Remove(resourceType);
        
        return true;
    }
    
    public bool RemoveGainedNature(string natureType, float cost)
    {
        if (!HasEnoughGainedNature(natureType, cost))
        {
            return false;
        }

        NatureGained[natureType] = NatureGained[natureType] - cost;
        
        return true;
    }

    // This function determines whether you have at least "amount" of a resource type
    public bool HasEnoughResources(string resourceType, int amount)
    {
        if (!resourcesOwned.ContainsKey(resourceType) || resourcesOwned[resourceType] < amount)
            return false;

        return true;
    }
    
    public bool HasEnoughGainedNature(string natureType, float amount)
    {
        if (!NatureGained.ContainsKey(natureType) || NatureGained[natureType] < amount)
            return false;

        return true;
    }

    // Displays the owned resources
    public void DisplayResources()
    {
        display.text = "Owned Nature:\n";

        foreach (KeyValuePair<string, int> keyValuePair in resourcesOwned)
        {
            display.text += "\n" + keyValuePair.Key + " (" + resourcesOwned[keyValuePair.Key] + ")";
        }
    }

    // Displays the items
    public void DisplayItems()
    {
        display.text += "\n\nCommodity:\n";
        
        foreach (var keyValuePair in itemsOwned)
        {
            display.text += "\n" + keyValuePair.Key + " (" + itemsOwned[keyValuePair.Key] + ")";
        }
    }

    // Used by the buttons to buy an item
    public void BuyItem(string item)
    {
        var successfulPurchase = false;

        switch (item.ToUpper())
        {
            case "IRON" :
                successfulPurchase = RemoveResource("LEAF", 10);
                break;
            case "GOLD" :
                successfulPurchase = RemoveResource("FLOWER", 2);
                break;
            case "OIL" :
                successfulPurchase = RemoveResource("STREAM", 10);
                break;
            case "DIAMOND" :
                successfulPurchase = RemoveResource("SUNLIGHT", 1);
                break;
        }

        if (successfulPurchase)
        {
            if (itemsOwned.ContainsKey(item.ToUpper()))
            {
                itemsOwned[item.ToUpper()] = itemsOwned[item.ToUpper()] + 1;
            }
            else
            {
                itemsOwned.Add(item.ToUpper(), 1);
            }
        }
    }
    
    public void ExploitNature(string resourceType)
    {
        var successfulPurchase = false;

        switch (resourceType.ToUpper())
        {
            case "LEAF" :
                successfulPurchase = RemoveGainedNature("RAWLEAF", 1);
                break;
            case "FLOWER" :
                successfulPurchase = RemoveGainedNature("RAWFLOWER", 1);
                break;
            case "STREAM" :
                successfulPurchase = RemoveGainedNature("RAWSTREAM", 1);
                break;
            case "SUNLIGHT" :
                successfulPurchase = RemoveGainedNature("RAWSUNLIGHT", 1);
                break;
        }

        if (successfulPurchase)
        {
            AddResource(resourceType, 1);
        }
    }
}
