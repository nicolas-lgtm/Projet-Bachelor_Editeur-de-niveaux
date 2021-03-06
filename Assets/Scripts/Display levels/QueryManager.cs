using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueryManager : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] GameObject windowsParent;

    //Objects
    [SerializeField] Toggle trap;
    [SerializeField] Toggle tree;
    [SerializeField] Toggle teleport;


    //Night level?
    [SerializeField] TMP_Dropdown nightLevel;

    //Turns limits
    [SerializeField] Toggle minTour;
    [SerializeField] Text minNb;

    [SerializeField] Toggle maxTour;
    [SerializeField] Text maxNb;

    string req;

    bool trapHasAlreadyBeenFiltered;
    bool filterAlreadyApplied;

    public string CreateQuery()
    {
        req = "SELECT * FROM `Level`";
        trapHasAlreadyBeenFiltered = false;
        filterAlreadyApplied = false;

        //TRAPS FILTERS
        if (trap.isOn || tree.isOn || teleport.isOn)
        {
                filterAlreadyApplied = true;

                req += " WHERE traps LIKE";

                if (trap.isOn)     FilterTrap("trap");
                if (tree.isOn)     FilterTrap("tree");
                if (teleport.isOn) FilterTrap("teleport");
        }

        //NIGHT FILTER
        if (nightLevel.value == 1 || nightLevel.value == 2)
        {
            if (filterAlreadyApplied) req += " AND";
            else req += " WHERE";

            req += " nightLevel = " + (nightLevel.value - 1).ToString();

            filterAlreadyApplied = true;
        }

        string req2 = req;

        //TOUR NB FILTERS
        if (minTour.isOn)
        {
            if (filterAlreadyApplied) req += " AND";
            else req += " WHERE";

            req += " max_turns >= " + minNb.text;

            filterAlreadyApplied = true;
        }

        if (maxTour.isOn)
        {
            if (filterAlreadyApplied) req += " AND";
            else req += " WHERE";

            req += " max_turns <= " + maxNb.text + " AND max_turns != 0";

            filterAlreadyApplied = true;
        }

        if (minTour.isOn && !maxTour.isOn) 
        {
            req += " UNION " + req2;

            if (!req.Contains("AND")) req += " WHERE";
            else req += " AND";

            req += " max_turns = 0";
        }

        req += " ORDER BY `level_name` ASC";

        return req;
    }

    void FilterTrap(string a_trapFilterName)
    {
        string toAdd = " '%" + a_trapFilterName + "%'";

        if (!trapHasAlreadyBeenFiltered)
        {
            req += toAdd;
            trapHasAlreadyBeenFiltered = true;
        }
        else
        {
            req += " AND traps LIKE " + toAdd;
        }
    }
}
