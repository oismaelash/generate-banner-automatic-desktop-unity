using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections;

namespace IsmaelNascimentoAssets
{
    public class BannerAutomatic : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private BannerModel bannerInfos;
        [Space(10)]
        [SerializeField] private string pathImageBackground;
        [SerializeField] private string pathCSV;
        [SerializeField] private string folderPathScreenshots;
        [Header("UI_REFS")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI dateTimeText;
        [SerializeField] private TextMeshProUGUI linkLiveText;

        private List<string> lineEvents = new List<string>();
        private List<BannerModel> banners = new List<BannerModel>();

        #endregion

        #region PRIVATE_METHODS

        [ContextMenu("CsvToBanner")]
        private void CsvToBanner()
        {
            Debug.Log("CsvToBanner start");

            using (var reader = new StreamReader(pathCSV.Replace("\\", "/")))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    //string[] values = line.Split(',');

                    lineEvents.Add(line);
                }
            }

            for (int index = 1; index < lineEvents.Count; index++)
            {
                banners.Add(new BannerModel()
                {
                    title = lineEvents[index].Split(',')[0],
                    speaker_name = lineEvents[index].Split(',')[1],
                    day = lineEvents[index].Split(',')[2],
                    month = lineEvents[index].Split(',')[3],
                    start_hour = lineEvents[index].Split(',')[4],
                    end_hour = lineEvents[index].Split(',')[5],
                    link_live = lineEvents[index].Split(',')[6]
                });
            }

            //banners.RemoveAt(0);

            StartCoroutine(ScreenshotsAll_Coroutine());

            //banners.ForEach(banner =>
            //{
            //    ChangeInformation(banner);
            //});

            Debug.Log("CsvToBanner end");
        }

        private void ChangeInformation(BannerModel bannerModel)
        {
            StartCoroutine(ChangeInformation_Coroutine(bannerModel));
        }

        #endregion

        #region COROUTINES

        private IEnumerator ChangeInformation_Coroutine(BannerModel bannerModel)
        {
            titleText.text = $"{bannerModel.title}";
            speakerNameText.text = $"{bannerModel.speaker_name}";
            dateTimeText.text = $"{bannerModel.day} de {bannerModel.month}, das {bannerModel.start_hour} - {bannerModel.end_hour}";
            linkLiveText.text = $"Assista em:\n{bannerModel.link_live}";
            yield return new WaitForSeconds(1f);
            string bannerName = $"{bannerModel.title}-{bannerModel.speaker_name}_BannerAutomic.PNG";
            ScreenCapture.CaptureScreenshot(Path.Combine(folderPathScreenshots, bannerName));
            Debug.Log($"New banner = {bannerName}");
        }

        private IEnumerator ScreenshotsAll_Coroutine()
        {
            for (int index = 0; index < banners.Count; index++)
            {
                titleText.text = $"{banners[index].title}";
                speakerNameText.text = $"{banners[index].speaker_name}";
                dateTimeText.text = $"{banners[index].day} de {banners[index].month}, das {banners[index].start_hour} - {banners[index].end_hour}";
                linkLiveText.text = $"Assista em:\n{banners[index].link_live}";
                yield return new WaitForSeconds(1f);
                string bannerName = $"{banners[index].title}-{banners[index].speaker_name}_BannerAutomic.PNG";
                ScreenCapture.CaptureScreenshot(Path.Combine(folderPathScreenshots, bannerName));
                Debug.Log($"New banner = {bannerName}");
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion
    }
}