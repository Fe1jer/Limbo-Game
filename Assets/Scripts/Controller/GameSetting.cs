using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Работа с интерфейсами
using UnityEngine.Audio; //Работа с аудио
using System.Linq;

public class GameSetting : MonoBehaviour
{
    public static GameSetting instance;
    [SerializeField]
    private GameController gameController;

    public GameObject buttonsMenu;
    public GameObject buttonsExit;
    public GameObject buttonsSetting;
    private AudioSource gameAudio; //Регулятор громкости
    public Toggle vSyncCount; //вертикальная синхронизация
    public Slider gameVolume; //громкость музыки
    public Toggle fullScreen; //Полноэкранный режим
    public Dropdown resolutionDropdown; //Список с разрешениями для игры

    private int vSync;
    private int quality = 0; //Качество
    private bool isFullscreen; //Полноэкранный режим
    private int currResolutionIndex; //Текущее разрешение

    void Start()
    {
        gameAudio = gameController.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Sound"))
        {
            gameAudio.volume = PlayerPrefs.GetFloat("Sound");
        }
        if (PlayerPrefs.HasKey("VSync"))
        {
            vSync = PlayerPrefs.GetInt("VSync");
            vSyncCount.isOn = vSync == 1;
            QualitySettings.vSyncCount = vSync;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            vSyncCount.isOn = true;
            vSync = QualitySettings.vSyncCount;
        }
        gameVolume.value = gameAudio.volume;
        BackInMenu();
        fullScreen.isOn = Screen.fullScreen;
        isFullscreen = fullScreen.isOn;
        ResolutionListInitialization();
    }

    void ResolutionListInitialization()
    {
        resolutionDropdown.ClearOptions(); //Удаление старых пунктов
        List<string> options = Screen.resolutions.Select(resolution => $"{resolution.width} x {resolution.height}").ToList();

        if (PlayerPrefs.HasKey("Resolutions"))
        {
            currResolutionIndex = PlayerPrefs.GetInt("Resolutions");
        }
        else
        {
            currResolutionIndex = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);

        }

        resolutionDropdown.AddOptions(options); //Добавление элементов в выпадающий список
        resolutionDropdown.value = currResolutionIndex; //Выделение пункта с текущим разрешением
        resolutionDropdown.RefreshShownValue(); //Обновление отображаемого значения
    }

    public void ShowExitButtons()
    {
        buttonsMenu.SetActive(false);
        buttonsExit.SetActive(true);
    }

    public void ShowSettings()
    {
        buttonsSetting.SetActive(true);
        buttonsMenu.SetActive(false);
    }

    public void BackInMenu()
    {
        buttonsMenu.SetActive(true);
        buttonsSetting.SetActive(false);
        buttonsExit.SetActive(false);
    }

    public void ChangeVolume(float val) //Изменение звука
    {
        gameAudio.volume = val;
    }

    public void VerticalSync(bool val) //Изменение разрешения
    {
        vSync = val ? 1 : 0 ;
    }

    public void ChangeResolution(int index) //Изменение разрешения
    {
        currResolutionIndex = index;
    }

    public void ChangeFullscreenMode(bool val) //Включение или отключение полноэкранного режима
    {
        isFullscreen = val;
    }

    public void ChangeQuality(int index) //Изменение качества
    {
        quality = index;
    }

    public void SaveSettings()
    {
        QualitySettings.vSyncCount = vSync;
        QualitySettings.SetQualityLevel(quality); //Изменение качества
        Screen.fullScreen = isFullscreen; //Включение или отключение полноэкранного режима
        Screen.SetResolution(Screen.resolutions[currResolutionIndex].width, Screen.resolutions[currResolutionIndex].height, isFullscreen); //Изменения разрешения
        PlayerPrefs.SetFloat("Sound", gameVolume.value);
        PlayerPrefs.SetInt("VSync", vSync);
        PlayerPrefs.SetInt("Resolutions", currResolutionIndex);
    }
}
