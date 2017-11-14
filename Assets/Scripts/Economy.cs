using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Economy : MonoBehaviour {

    // The slider used as a bar representation of the economy
    [SerializeField]
    private Slider m_slider;
    // This allows for multiple economy types. When the ZeroEconomy function is called it will send the corresponding ID, allowing individual scripts to have a switch
    // that will deal with specific economies;
    [SerializeField]
    private int m_economyID;
    // These object will be sent a message when the economy is equal to zero, if there is no object the message will be sent to the GameObject this script is attached to
    // You could use this to tell an object when to "die" and also activate game over screens etc.
    public GameObject[] m_zeroEconomyTargets;
    // Buffer zone, can be used to make the last bit of health feel like it's worth more.
    // Example: a buffer zone of 5% and a buffer multiplier value of .5 will reduced incoming reductions to half damage for the last 5% of the health bar
    [SerializeField]
    private bool m_useReductionBufferZone;
    [SerializeField]
    [Range(0, 100)] private int m_bufferPercent;
    [SerializeField]
    [Range(0, 1)] private float m_bufferMultiplier;
    // Basic economy values
    [SerializeField]
    private float m_startValue;
    [SerializeField]
    private float m_currentValue;
    [SerializeField]
    private float m_maxValue;
    // Regenerative values, useful for health/mana etc that recovers after a specific period of not being hit/used
    [SerializeField]
    private bool m_regenerative;
    [SerializeField]
    private float m_regenerationTickRate;
    [SerializeField]
    private float m_regenerationValuePerTick;
    [SerializeField]
    private float m_regenerationWaitTime;
    // Primary Methods
    public void Add(float value)
    {
        m_currentValue += value;
        if (m_currentValue > m_maxValue)
        {
            m_currentValue = m_maxValue;
        }
        UpdateSlider();
    }
    public void Subtract(float value)
    {

        if (m_useReductionBufferZone) {

            float remainingHealthPercent = Percentage(m_currentValue, m_maxValue);
            if (remainingHealthPercent <= m_bufferPercent)
            {
                value = value * m_bufferMultiplier;
            }

        }

        m_currentValue -= value;

        m_timeSinceLastReduction = 0;
        if (m_currentValue <= 0)
        {
            ZeroEconomy();
        }
        UpdateSlider();

    }
    private float Percentage(float val1, float val2)
    {
        float value = val1 / val2 * 100;
        return value;
    }
    public void ZeroEconomy()
    {
        if (m_currentValue != 0)
        {
            m_currentValue = 0;
        }
        if (m_zeroEconomyTargets.Length > 0)
        {
            for (int i = 0; i < m_zeroEconomyTargets.Length; i++)
            {
                m_zeroEconomyTargets[i].BroadcastMessage("EconomyUsed",m_economyID, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            this.gameObject.BroadcastMessage("EconomyUsed", m_economyID, SendMessageOptions.DontRequireReceiver);
        }
        UpdateSlider();
    }
    // Timers    
    private float m_timeSinceLastTick;
    private float m_timeSinceLastReduction;
    void FixedUpdate()
    {
        // If you have an economy you want to begin regenerating just add 1 to the value.
        // Good for, opening up a mana bar for the first time and watching it "fill"
        if (m_regenerative && m_currentValue > 0)
        {

            m_timeSinceLastReduction += Time.deltaTime;
            m_timeSinceLastTick += Time.deltaTime;

            // 1. Check if we're at full economy value so we skip this code if it's not needed
            if (m_currentValue >= m_maxValue)
            {
                return;
            }

            // 2. Check how long it's been since we've taken a reduction
            if (m_timeSinceLastReduction > m_regenerationWaitTime)
            {
                // 3. Check how long it's been since we've added a tick
                if (m_timeSinceLastTick >= m_regenerationTickRate)
                {

                    // 4. Add a tick and resent the time since last tick
                    Add(m_regenerationValuePerTick);
                    m_timeSinceLastTick = 0;

                }

            }

        }

    }
    void Start()
    {
        m_currentValue = m_startValue;
    }
    // UpdateSlider
    void UpdateSlider()
    {
        if (m_slider)
        {
            // turns the min max value to a range from 0 - 1
            m_slider.value = m_currentValue / m_maxValue;
        }
    }
}
