﻿using UnityEngine;
using System.Collections;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource m_audioSource;
    public AudioClip m_themeMusic;
    public AudioClip m_cocoonMusic;

    void Awake()
    {
        if(m_audioSource == null) m_audioSource = gameObject.AddComponent<AudioSource>();
        PlayCocoonMusic();
    }

    public void PlayThemeMusic()
    {
        if (m_audioSource.clip != m_themeMusic || !m_audioSource.isPlaying)
        {
            m_audioSource.clip = m_themeMusic;
            m_audioSource.Play();
        }
    }

    public void PlayCocoonMusic()
    {
        if (m_audioSource.clip != m_cocoonMusic || !m_audioSource.isPlaying)
        {
            m_audioSource.clip = m_cocoonMusic;
            m_audioSource.Play();
        }
    }

}
