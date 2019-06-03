using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UTimer {

	public float time;
	public bool finished = true;
	public bool hasBeenLaunched = false;
	private bool debug = false;
	private float cooldown = 0;
	private Coroutine instanceCoroutine;
	private MonoBehaviour mono;
	private float timerPrecision = 0.01f;
    private UnityAction callbackFunction;

	private UTimer(float time, MonoBehaviour obj, UnityAction function, bool debug){
		this.time = time;
		this.debug = debug;
		this.mono = obj;
        callbackFunction = function;
	}

	//timer : the time took by the timer to switch <finished> to true
	//obj : need a monobehaviour reference to lanch the StartCoroutine function, almost every time 'this' (i know, a little ugly)
	//debug : if you want to see the timer developpement
	public static UTimer Initialize(float time, MonoBehaviour obj, UnityAction function, bool debug = false){
		UTimer returnVal = new UTimer(time, obj, function, debug);
		return returnVal;
	}

	public void Stop (bool deb = false){
		this.debug = deb;

        if(instanceCoroutine != null)
		    mono.StopCoroutine(instanceCoroutine);
		finished = false;
		hasBeenLaunched = false;
		cooldown = 0;
		if(debug)
			Debug.Log("coroutine " + instanceCoroutine.ToString() + " stopped.");
	}

	public void start(bool deb = false){
		this.debug = deb;
		if(instanceCoroutine != null)
            mono.StopCoroutine(instanceCoroutine);
		cooldown = 0;
		finished = false;
		hasBeenLaunched = true;
		if(debug)
			Debug.Log("Start");
		instanceCoroutine = mono.StartCoroutine(LaunchCooldown());
	}

	public void start(float time, bool deb = false){
		this.debug = deb;
		if(instanceCoroutine != null)
            mono.StopCoroutine(instanceCoroutine);
		cooldown = 0;
		finished = false;
		this.time = time;
		if(debug)
			Debug.Log("Start");
		instanceCoroutine = mono.StartCoroutine(LaunchCooldown());
	}

    public void pause(bool deb = false)
    {
        this.debug = deb;
        if (instanceCoroutine != null)
            mono.StopCoroutine(instanceCoroutine);
        finished = false;
        hasBeenLaunched = false;
        if (debug)
            Debug.Log("coroutine " + instanceCoroutine.ToString() + " paused.");
    }

    public void continu(bool deb = false)
    {
        this.debug = deb;
        if (instanceCoroutine != null)
            mono.StopCoroutine(instanceCoroutine);
        finished = false;
        hasBeenLaunched = true;
        if (debug)
            Debug.Log("Continue");
        instanceCoroutine = mono.StartCoroutine(LaunchCooldown());
    }

    public void restart(){
		Stop();
		start();
	}

	public void restart(float time){
		Stop();
		start(time);
	}

	private IEnumerator LaunchCooldown(){
		while (cooldown < time){
			if(debug)
				Debug.Log("Countdown: " + cooldown);
            //yield return new WaitForSecondsRealtime(timerPrecision);
            //cooldown += timerPrecision;
            cooldown += Time.deltaTime;
            yield return null;
		}
		finished = true;
        callbackFunction();

    } 

    public float getCooldown()
    {
        return cooldown;
    }

    public void setCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

}
