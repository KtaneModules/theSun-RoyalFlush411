using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using theSun;

public class theSunScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] buttons;
    public Renderer[] leds;
    public Light[] lights;

    //LED info
    private int ledIndex = 0;
    private int numberOfModules = 0;
    public string[] ledLocations;

    //Sections
    private int indicators = 0;
    private int consonants = 0;
    private int batteries = 0;
    private int digits = 0;
    private int ports = 0;
    private int battHolders = 0;
    private int portPlates = 0;
    private int lightCount = 0;

    //Correct button list
    private int startPosition = 0;
    private List<KMSelectable> correctButtons = new List<KMSelectable>();
    private List<KMSelectable> correctButtonsOrdered = new List<KMSelectable>();
    private List<Light> correctLights = new List<Light>();
    private List<Light> correctLightsOrdered = new List<Light>();
    private List<int> serialNumberEntries = new List<int>();
    private List<int> serialNumberEntriesModulo = new List<int>();
    private int rotationClock = 1;
    private int rotationCounter = 1;

    //Logging etc.
    static int moduleIdCounter = 1;
    int moduleId;
    private int stage = 1;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons)
        {
            KMSelectable trueButton = button;
            button.OnInteract += delegate () { Onbutton(trueButton); return false; };
        }
    }

    void Start()
    {
        numberOfModules = Bomb.GetModuleNames().Count();
        foreach (Renderer led in leds)
        {
            led.enabled = false;
        }
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        startPosition = numberOfModules % 8;
        LEDPicker();
        SectionDetermination();
        SelectionOrder();
    }

    void LEDPicker()
    {
        ledIndex = UnityEngine.Random.Range(0,8);
        leds[ledIndex].enabled = true;
        Debug.LogFormat("[The Sun #{0}] The LED is in the {1} position.", moduleId, ledLocations[ledIndex]);
    }

    void SectionDetermination()
    {
        indicators = (Bomb.GetOnIndicators().Count() + Bomb.GetOffIndicators().Count()) % 7;
        consonants = (Bomb.GetSerialNumberLetters().Count(x => x != 'A' && x != 'E' && x != 'I' && x != 'O' && x != 'U')) % 7;
        batteries = Bomb.GetBatteryCount() % 7;
        digits = Bomb.GetSerialNumberNumbers().Count() % 7;
        ports = Bomb.GetPortCount() % 7;
        battHolders = Bomb.GetBatteryHolderCount() % 7;
        portPlates = Bomb.GetPortPlates().Count() % 7;
        Debug.LogFormat("[The Sun #{0}] # of indicators modulo 7 = {1}.", moduleId, indicators);
        Debug.LogFormat("[The Sun #{0}] # of consonants modulo 7 = {1}.", moduleId, consonants);
        Debug.LogFormat("[The Sun #{0}] # of batteries modulo 7 = {1}.", moduleId, batteries);
        Debug.LogFormat("[The Sun #{0}] # of digits modulo 7 = {1}.", moduleId, digits);
        Debug.LogFormat("[The Sun #{0}] # of ports modulo 7 = {1}.", moduleId, ports);
        Debug.LogFormat("[The Sun #{0}] # of modules modulo 7 = {1}.", moduleId, numberOfModules % 7);
        Debug.LogFormat("[The Sun #{0}] # of battery holders modulo 7 = {1}.", moduleId, battHolders);
        Debug.LogFormat("[The Sun #{0}] # of port plates modulo 7 = {1}.", moduleId, portPlates);

        if (ledIndex == 0)
        {
            indicatorsMethod();
        }
        else if (ledIndex == 7)
        {
            consonantsMethod();
        }
        else if (ledIndex == 6)
        {
            batteriesMethod();
        }
        else if (ledIndex == 5)
        {
            digitsMethod();
        }
        else if (ledIndex == 4)
        {
            portsMethod();
        }
        else if (ledIndex == 3)
        {
            modulesMethod();
        }
        else if (ledIndex == 2)
        {
            battHoldersMethod();
        }
        else if (ledIndex == 1)
        {
            portPlatesMethod();
        }
    }

    void indicatorsMethod()
    {
        if (indicators < 2)
        {
            correctButtons.Add(buttons[ledIndex]);
            correctLights.Add(lights[ledIndex]);
        }
        else if (indicators >= 2 && indicators < 5)
        {
            correctButtons.Add(buttons[ledIndex + 8]);
            correctLights.Add(lights[ledIndex + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            consonantsMethod();
        }
    }

    void consonantsMethod()
    {
        if (consonants < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 1) % 8]);
            correctLights.Add(lights[(ledIndex + 1) % 8]);
        }
        else if (consonants >= 2 && consonants < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 1) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 1) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            batteriesMethod();
        }
    }

    void batteriesMethod()
    {
        if (batteries < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 2) % 8]);
            correctLights.Add(lights[(ledIndex + 2) % 8]);
        }
        else if (batteries >= 2 && batteries < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 2) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 2) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            digitsMethod();
        }
    }

    void digitsMethod()
    {
        if (digits < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 3) % 8]);
            correctLights.Add(lights[(ledIndex + 3) % 8]);
        }
        else if (digits >= 2 && digits < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 3) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 3) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            portsMethod();
        }
    }

    void portsMethod()
    {
        if (ports < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 4) % 8]);
            correctLights.Add(lights[(ledIndex + 4) % 8]);
        }
        else if (ports >= 2 && ports < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 4) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 4) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            modulesMethod();
        }
    }

    void modulesMethod()
    {
        if (numberOfModules % 7 < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 5) % 8]);
            correctLights.Add(lights[(ledIndex + 5) % 8]);
        }
        else if (numberOfModules % 7 >= 2 && numberOfModules % 7 < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 5) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 5) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            battHoldersMethod();
        }
    }

    void battHoldersMethod()
    {
        if (battHolders < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 6) % 8]);
            correctLights.Add(lights[(ledIndex + 6) % 8]);
        }
        else if (battHolders >= 2 && battHolders < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 6) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 6) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            portPlatesMethod();
        }
    }

    void portPlatesMethod()
    {
        if (portPlates < 2)
        {
            correctButtons.Add(buttons[(ledIndex + 7) % 8]);
            correctLights.Add(lights[(ledIndex + 7) % 8]);
        }
        else if (portPlates >= 2 && portPlates < 5)
        {
            correctButtons.Add(buttons[((ledIndex + 7) % 8) + 8]);
            correctLights.Add(lights[((ledIndex + 7) % 8) + 8]);
        }
        else
        {
            correctButtons.Add(buttons[16]);
            correctLights.Add(lights[16]);
        }
        if (correctButtons.Count() == 8)
        {
            return;
        }
        else
        {
            indicatorsMethod();
        }
    }

    void SelectionOrder()
    {
        serialNumberEntries = (Bomb.GetSerialNumber().Select(c => Char.IsDigit(c) ? c - '0' : c - '@')).ToList();
        foreach (int digit in serialNumberEntries)
        {
            serialNumberEntriesModulo.Add(digit % 10);
        }

        correctButtonsOrdered.Add(correctButtons[startPosition]);
        correctLightsOrdered.Add(correctLights[startPosition]);
        OrderingLogic();
    }

    void OrderingLogic()
    {
        foreach (int digit in serialNumberEntriesModulo)
        {
            if (digit > 4)
            {
                correctButtonsOrdered.Add(correctButtons[(startPosition + rotationClock) % 8]);
                correctLightsOrdered.Add(correctLights[(startPosition + rotationClock) % 8]);
                rotationClock++;
            }
            else if (digit < 5)
            {
                correctButtonsOrdered.Add(correctButtons[((startPosition - rotationCounter) + 8) % 8]);
                correctLightsOrdered.Add(correctLights[((startPosition - rotationCounter) + 8) % 8]);
                rotationCounter++;
            }
        }
        correctButtonsOrdered.Add(correctButtons[(startPosition + rotationClock) % 8]);
        correctLightsOrdered.Add(correctLights[(startPosition + rotationClock) % 8]);
        Debug.LogFormat("[The Sun #{0}] The first button is {1}.", moduleId, correctButtonsOrdered[0].name);
        if (correctButtonsOrdered[0] != buttons[16])
        {
            Debug.LogFormat("[The Sun #{0}] The second button is {1}.", moduleId, correctButtonsOrdered[1].name);
            if (correctButtonsOrdered[1] != buttons[16])
            {
                Debug.LogFormat("[The Sun #{0}] The third button is {1}.", moduleId, correctButtonsOrdered[2].name);
                if (correctButtonsOrdered[2] != buttons[16])
                {
                    Debug.LogFormat("[The Sun #{0}] The fourth button is {1}.", moduleId, correctButtonsOrdered[3].name);
                    if (correctButtonsOrdered[3] != buttons[16])
                    {
                        Debug.LogFormat("[The Sun #{0}] The fifth button is {1}.", moduleId, correctButtonsOrdered[4].name);
                        if (correctButtonsOrdered[4] != buttons[16])
                        {
                            Debug.LogFormat("[The Sun #{0}] The sixth button is {1}.", moduleId, correctButtonsOrdered[5].name);
                            if (correctButtonsOrdered[5] != buttons[16])
                            {
                                Debug.LogFormat("[The Sun #{0}] The seventh button is {1}.", moduleId, correctButtonsOrdered[6].name);
                                if (correctButtonsOrdered[6] != buttons[16])
                                {
                                    Debug.LogFormat("[The Sun #{0}] The eighth button is {1}.", moduleId, correctButtonsOrdered[7].name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Onbutton(KMSelectable button)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();

        switch (stage)
        {
            case 1:
            if (button == correctButtonsOrdered[0])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[0].name);
                if (correctButtonsOrdered[0] != buttons[16])
                {
                    correctLightsOrdered[0].enabled = true;
                    Audio.PlaySoundAtTransform("tone1", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[0].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 2:
            if (button == correctButtonsOrdered[1])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[1].name);
                if (correctButtonsOrdered[1] != buttons[16])
                {
                    correctLightsOrdered[1].enabled = true;
                    Audio.PlaySoundAtTransform("tone2", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[1].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 3:
            if (button == correctButtonsOrdered[2])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[2].name);
                if (correctButtonsOrdered[2] != buttons[16])
                {
                    correctLightsOrdered[2].enabled = true;
                    Audio.PlaySoundAtTransform("tone3", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[2].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 4:
            if (button == correctButtonsOrdered[3])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[3].name);
                if (correctButtonsOrdered[3] != buttons[16])
                {
                    correctLightsOrdered[3].enabled = true;
                    Audio.PlaySoundAtTransform("tone4", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[3].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 5:
            if (button == correctButtonsOrdered[4])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[4].name);
                if (correctButtonsOrdered[4] != buttons[16])
                {
                    correctLightsOrdered[4].enabled = true;
                    Audio.PlaySoundAtTransform("tone5", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[4].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 6:
            if (button == correctButtonsOrdered[5])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[5].name);
                if (correctButtonsOrdered[5] != buttons[16])
                {
                    correctLightsOrdered[5].enabled = true;
                    Audio.PlaySoundAtTransform("tone6", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[5].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 7:
            if (button == correctButtonsOrdered[6])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[6].name);
                if (correctButtonsOrdered[6] != buttons[16])
                {
                    correctLightsOrdered[6].enabled = true;
                    Audio.PlaySoundAtTransform("tone7", transform);
                    stage++;
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[6].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            case 8:
            if (button == correctButtonsOrdered[7])
            {
                Debug.LogFormat("[The Sun #{0}] You pressed {1}. That is correct.", moduleId, correctButtonsOrdered[7].name);
                if (correctButtonsOrdered[7] != buttons[16])
                {
                    correctLightsOrdered[7].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    stage++;
                    ModuleSolved();
                }
                else
                {
                    lights[16].enabled = true;
                    Audio.PlaySoundAtTransform("tone8", transform);
                    ModuleSolved();
                }
            }
            else
            {
                Debug.LogFormat("[The Sun #{0}] Strike! You pressed {1}. That is incorrect. I was expecting {2}.", moduleId, button.name, correctButtonsOrdered[7].name);
                stage = 1;
                foreach (Light light in lights)
                {
                    light.enabled = false;
                }
                GetComponent<KMBombModule>().HandleStrike();
            }
            break;

            default:
                break;
        }
    }

    void ModuleSolved()
    {
        stage = 9;
        StartCoroutine(lightDance());
        Debug.LogFormat("[The Sun #{0}] Module disarmed.", moduleId);
        GetComponent<KMBombModule>().HandlePass();
    }

    private IEnumerator lightDance()
    {
        yield return new WaitForSeconds(0.3f);

        if (lightCount == 0)
        {
            Audio.PlaySoundAtTransform("toneFinish", transform);
        }

        while (lightCount < 4)
        {
            yield return new WaitForSeconds(0.25f);
            foreach (Light light in correctLightsOrdered)
            {
                light.enabled = false;
            }
            lights[0].enabled = true;
            lights[2].enabled = true;
            lights[4].enabled = true;
            lights[6].enabled = true;
            lights[8].enabled = true;
            lights[10].enabled = true;
            lights[12].enabled = true;
            lights[14].enabled = true;
            lights[1].enabled = false;
            lights[3].enabled = false;
            lights[5].enabled = false;
            lights[7].enabled = false;
            lights[9].enabled = false;
            lights[11].enabled = false;
            lights[13].enabled = false;
            lights[15].enabled = false;
            lights[16].enabled = false;

            yield return new WaitForSeconds(0.25f);
            lights[0].enabled = false;
            lights[2].enabled = false;
            lights[4].enabled = false;
            lights[6].enabled = false;
            lights[8].enabled = false;
            lights[10].enabled = false;
            lights[12].enabled = false;
            lights[14].enabled = false;
            lights[1].enabled = true;
            lights[3].enabled = true;
            lights[5].enabled = true;
            lights[7].enabled = true;
            lights[9].enabled = true;
            lights[11].enabled = true;
            lights[13].enabled = true;
            lights[15].enabled = true;
            lights[16].enabled = true;
            lightCount++;
        }
        lights[0].enabled = true;
        lights[2].enabled = true;
        lights[4].enabled = true;
        lights[6].enabled = true;
        lights[8].enabled = true;
        lights[10].enabled = true;
        lights[12].enabled = true;
        lights[14].enabled = true;
        lights[1].enabled = true;
        lights[3].enabled = true;
        lights[5].enabled = true;
        lights[7].enabled = true;
        lights[9].enabled = true;
        lights[11].enabled = true;
        lights[13].enabled = true;
        lights[15].enabled = true;
        lights[16].enabled = true;
    }
    
#pragma warning disable 414
    private string TwitchHelpMessage = @"Use “!{0} press inner top” to press the inner top button. Use “!{0} press outer bottomleft” to press the outer top left button. Use “!{0} press center” to press the center button. Combine the commands using a colon (;). NEWS directions (North-East-South-West) and shortened directions (“t” and “n”) also work.";
#pragma warning restore 414

    private static string[] supportedSections = new[] { "inner", "outer" };
    private static string[] supportedDirections = new[] { "top", "bottom", "left", "right", "topleft", "topright", "bottomleft", "bottomright", "north", "south", "east", "west", "northwest", "northeast", "southwest", "southeast", "n", "s", "e", "w", "nw", "ne", "sw", "se", "t", "b", "l", "r", "tl", "bl", "tr", "br" };

    IEnumerator ProcessTwitchCommand(string command)
    {
        var parts = command.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length > 1 && parts[0] == "press")
        {
            var cmdButtons = command.ToLowerInvariant().Replace("press ", "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            bool goodCommands = true;

            foreach (string cmd in cmdButtons)
            {
                if (!((cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length == 1 && cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0] == "center") || (cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length == 2 && supportedSections.Contains(cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]) && supportedDirections.Contains(cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]))))
                {
                    goodCommands = false;
                }
            }

            if (goodCommands)
            {
                yield return null;

                foreach (string cmd in cmdButtons)
                {
                    var split = cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length == 1)
                    {
                        CheckAndPress("center", "");
                    }
                    else
                    {
                        CheckAndPress(split[0], split[1]);
                    }
                    
                    yield return new WaitForSeconds(.2f);
                }
                yield break;
            }
            
        }
    }

    void CheckAndPress(string section, string direction)
    {
        if (new[] { "top", "t", "north", "n" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[0]);
            }
            else
            {
                Onbutton(buttons[8]);
            }
        }
        else if (new[] { "topright", "tr", "northeast", "ne" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[1]);
            }
            else
            {
                Onbutton(buttons[9]);
            }
        }
        else if (new[] { "right", "r", "east", "e" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[2]);
            }
            else
            {
                Onbutton(buttons[10]);
            }
        }
        else if (new[] { "bottomright", "br", "southeast", "se" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[3]);
            }
            else
            {
                Onbutton(buttons[11]);
            }
        }
        else if (new[] { "bottom", "b", "south", "s" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[4]);
            }
            else
            {
                Onbutton(buttons[12]);
            }
        }
        else if (new[] { "bottomleft", "bl", "southwest", "sw" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[5]);
            }
            else
            {
                Onbutton(buttons[13]);
            }
        }
        else if (new[] { "left", "l", "west", "w" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[6]);
            }
            else
            {
                Onbutton(buttons[14]);
            }
        }
        else if (new[] { "topleft", "tl", "northwest", "nw" }.Contains(direction))
        {
            if (section == "outer")
            {
                Onbutton(buttons[7]);
            }
            else
            {
                Onbutton(buttons[15]);
            }
        }
        else if (section == "center")
        {
            Onbutton(buttons[16]);
        }
    }
}
