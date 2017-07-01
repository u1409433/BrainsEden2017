using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringAudio : MonoBehaviour
{
        void HowToTriggerAudio()
    {
        //To call a sound effect:

        AkSoundEngine.PostEvent("Name of Event", gameObject);   //the EXACT names of the events can be found in the Wwise Picker at the bottom of the "Windows" dropdown in Unity
                                                                //The Wwise Picker contains the events and the soundbank (from which all the audio is loaded). These can
                                                                //all be dragged to the relevant gameObject so they can be called in script. For example, if you want to call
                                                                //sound events from a script, drag the event(s) you wish to call from the Wwise Picker to the relevent GameObject;
                                                                //then also drag the "Main" soundbank below that. Both of these should be freaky looking components on the object
                                                                //now along with an "Ak Game Obj" component. If there is a red warning about being environment aware, untick the 
                                                                //option "Environment Aware" and then make sure that the event's "Trigger On" is set to "Nothing". The soundbank
                                                                //("AkBank") should load on Start.

        //e.g. to begin the ambient music:

        AkSoundEngine.PostEvent("Ambience", gameObject);    //the gameObject should be the GO to which the event script has been added

        //Switches change what is being played. To change a switch:

        AkSoundEngine.SetSwitch("Name of Switch Group", "Name of Switch State", gameObject);    //again make sure the event that is tied to to the Switch group is dragged onto
                                                                                                //the object from the Wwise Picker (for us the Event and the Switch group will
                                                                                                //have the same names). The names of the different groups and states are in the
                                                                                                //Wwise picker as well of course

        //To change the ambient music because of entering the spawn area box trigger collider (could just be on a door way or encompassing the whole section):

        AkSoundEngine.SetSwitch("Ambience", "Spawn", gameObject);
    }
}
