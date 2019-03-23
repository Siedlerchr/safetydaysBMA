﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;

public class inputScreen : MonoBehaviour
{

    List<string> melderart_list = new List<string> { "Brand", "Rauch", "CO" };
    List<string> timedelay_list = new List<string> { "0", "30", "60", "90" };

    public Dropdown m_Dropdown_Melderart;
    public Dropdown m_Dropdown_TimeDelay;

    public Toggle m_automatisch;
    public Toggle m_handmelder;
    public Toggle m_loeschanlage;
    public Toggle m_fehlalarm;

    public InputField m_meldergruppe;
    public InputField m_meldernummer;
    public InputField m_hinweistext;
    public InputField m_freitext;

    public Text timedelayLbl;
    public Text infoText;

    public int alarmid;

    public int firstRun = 0;

    public AlarmList alarmList;

    // Start is called before the first frame update
    void Start()
    {
        firstRun = 0;

        initializeMelderart();
        initializeTimeDelay();

        m_handmelder.isOn = false;
        m_loeschanlage.isOn = false;
        m_automatisch.isOn = true;

        m_fehlalarm.isOn = false;

        m_hinweistext.text = "Brandalarm";

    }

    // Update is called once per frame
    void Update()
    {

    }

    void initializeMelderart()
    {
        m_Dropdown_Melderart.ClearOptions();
        m_Dropdown_Melderart.AddOptions(melderart_list);
    }

    void initializeTimeDelay()
    {
        m_Dropdown_TimeDelay.ClearOptions();
        m_Dropdown_TimeDelay.AddOptions(timedelay_list);
    }

    void generateScenarioFromInput()
    {
        Alarm.MelderType meldertyp = Alarm.MelderType.Melder;
        Alarm.AlarmType alarmtyp = Alarm.AlarmType.Alarm;

        //count up alarmid
        alarmid++;
        //extract meldertyp from checkboxes
        if (m_handmelder.isOn)
        {
            //for alarm reasons only "Melder" exists
            meldertyp = Alarm.MelderType.Melder;
        }
        if (m_loeschanlage.isOn)
        {
            meldertyp = Alarm.MelderType.Loeschanlage;
        }
        if (m_automatisch.isOn)
        {
            meldertyp = Alarm.MelderType.Melder;
        }

        //check if fehlalarm is active
        if (m_fehlalarm.isOn)
        {
            alarmtyp = Alarm.AlarmType.FalseAlarm;
        }

        //convert timedelay from string of dropdown to int
        int timedelay;
        int.TryParse(timedelayLbl.text, out timedelay);

        alarmList.gameObject.SetActive(true);
        alarmList.addAlarm(new Alarm(alarmid, timedelay, meldertyp, m_hinweistext.text, m_freitext.text, alarmtyp));
        infoText.text = "Meldung wurde der Alarm-Liste hinzugefügt.";

    }

    public void saveData()
    {
        //check if neccessary fields are filled
        if (m_meldernummer.text == "" || m_meldergruppe.text == "" || m_freitext.text == "" || m_hinweistext.text == "")
        {
            //EditorUtility.DisplayDialog("Oh man!", "Alle fettgedruckten Felder füllen", "Okay");
        }
        else
        {
            // go!
            generateScenarioFromInput();
        }
    }

    public void readyButton()
    {
        saveData();

    }

    public void setOnlyOneToggleActive()
    {
        if (firstRun > 0)
        {

            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "Automatisch_Tgl":
                    if (m_automatisch.isOn == false)
                    {
                        m_automatisch.isOn = true;
                    }
                    else
                    {
                        m_handmelder.isOn = false;
                        m_loeschanlage.isOn = false;
                    }
                    break;
                case "Loeschanlage_Tgl":
                    if (m_loeschanlage.isOn == false)
                    {
                        m_loeschanlage.isOn = true;
                    }
                    else
                    {
                        m_handmelder.isOn = false;
                        m_automatisch.isOn = false;
                    }
                    break;
                case "Handmelder_Tgl":
                    if (m_handmelder.isOn == false)
                    {
                        m_handmelder.isOn = true;
                    }
                    else
                    {
                        m_loeschanlage.isOn = false;
                        m_automatisch.isOn = false;
                    }
                    break;
                default:
                    break;
            }
        }

        firstRun++;
    }
}
