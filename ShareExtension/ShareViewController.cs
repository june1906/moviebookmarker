using System;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using MobileCoreServices;
using Social;
using UIKit;
using MetaData;
using Datamodel;
using SQLite;
using HtmlAgilityPack;
using System.Security.Policy;
using System.IO;
using System.Text.RegularExpressions;

namespace ShareExtension
{
    
    public partial class ShareViewController : UIViewController
    {
        
        UIButton addbutton, cancelbutton;
        UITextView titleLabel, tenphimLabel, tenphimTextViewEx, sotapLabel, sotapTextViewEx, sophutLabel, sophutTextViewEx;
        UIImageView imageViewEx;
        SQLiteConnection db;
        protected ShareViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }
        public override bool ShouldAutorotate()
        {
            return false;
        }
        

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }


        public override void ViewDidLoad()
        {
            
           
                nfloat h = 31.0f;
                nfloat w = View.Bounds.Width;
                base.ViewDidLoad();


                imageViewEx = new UIImageView(new CGRect(10, 100, 150, 200));
                Add(imageViewEx);

                titleLabel = new UITextView();
                titleLabel.Frame = new CGRect(110, 40, w - 180, 50);
                titleLabel.Font = UIFont.SystemFontOfSize(22);
                titleLabel.Text = "ADD NEW MOVIE";
                titleLabel.Editable = false;
                titleLabel.TextColor = UIColor.Brown;

                View.AddSubview(titleLabel);

                tenphimLabel = new UITextView();
                tenphimLabel.Frame = new CGRect(170, 90, w - 180, 30);
                tenphimLabel.Font = UIFont.SystemFontOfSize(18);
                tenphimLabel.Text = "Movie's name:";
                tenphimLabel.Editable = false;
                tenphimLabel.ScrollEnabled = false;
                tenphimLabel.ClipsToBounds = true;
                tenphimLabel.TextColor = UIColor.Brown;
                //tenphimLabel.TextAlignment = UITextAlignment.Center;
                View.AddSubview(tenphimLabel);

                tenphimTextViewEx = new UITextView();
                tenphimTextViewEx.Frame = new CGRect(170, 140, w - 180, 100);
                tenphimTextViewEx.TextAlignment = UITextAlignment.Center;
                tenphimTextViewEx.Font = UIFont.SystemFontOfSize(18);
                tenphimTextViewEx.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

                // limit of up to 50 characters
                /*tenphimTextViewEx.ShouldChangeCharacters = (textField, range, replacementString) =>
                {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 40;
                };*/
                View.AddSubview(tenphimTextViewEx);

                sotapLabel = new UITextView();
                sotapLabel.Frame = new CGRect(170, 250, w - 180, 30);
                sotapLabel.Font = UIFont.SystemFontOfSize(18);
                sotapLabel.Text = "Episodes:";
                sotapLabel.Editable = false;
                sotapLabel.ScrollEnabled = false;
                sotapLabel.ClipsToBounds = true;
                sotapLabel.TextColor = UIColor.Brown;
                //tenphimLabel.TextAlignment = UITextAlignment.Center;
                View.AddSubview(sotapLabel);

                sotapTextViewEx = new UITextView();
                sotapTextViewEx.Frame = new CGRect(170, 290, w - 180, 30);
                sotapTextViewEx.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                sotapTextViewEx.Font = UIFont.SystemFontOfSize(18);
                sotapTextViewEx.TextAlignment = UITextAlignment.Center;
                sotapTextViewEx.KeyboardType = UIKeyboardType.NumberPad;

                /*sotapTextViewEx.ShouldChangeCharacters = (textField, range, replacementString) =>
                {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 3;
                };*/
                View.AddSubview(sotapTextViewEx);

                sophutLabel = new UITextView();
                sophutLabel.Frame = new CGRect(10, 330, w - 20, 30);
                sophutLabel.Font = UIFont.SystemFontOfSize(18);
                sophutLabel.Text = "Minutes watching:";
                sophutLabel.ScrollEnabled = false;
                sophutLabel.ClipsToBounds = true;
                sophutLabel.Editable = false;
                sophutLabel.TextColor = UIColor.Brown;
                //tenphimLabel.TextAlignment = UITextAlignment.Center;
                View.AddSubview(sophutLabel);

                sophutTextViewEx = new UITextView();
                sophutTextViewEx.Frame = new CGRect(10, 370, w - 20, h);
                sophutTextViewEx.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                sophutTextViewEx.TextAlignment = UITextAlignment.Center;
                sophutTextViewEx.Font = UIFont.SystemFontOfSize(18);
                sophutTextViewEx.KeyboardType = UIKeyboardType.NumberPad;
                /*sophutTextViewEx.ShouldChangeCharacters = (textField, range, replacementString) =>
                {
                    var newLength = textField.Text.Length + replacementString.Length - range.Length;
                    return newLength <= 4;
                };*/
                View.AddSubview(sophutTextViewEx);



                addbutton = new UIButton();
                addbutton.Frame = new CGRect(10, 450, w - 20, 50);
                addbutton.SetTitle("ADD NEW MOVIE", UIControlState.Normal);
                addbutton.BackgroundColor = UIColor.Blue;
                addbutton.Layer.CornerRadius = 5f;
                View.AddSubview(addbutton);
                addbutton.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

                cancelbutton = new UIButton();
                cancelbutton.Frame = new CGRect(10, 515, w - 20, 50);
                cancelbutton.SetTitle("CANCEL", UIControlState.Normal);
                cancelbutton.BackgroundColor = UIColor.Red;
                cancelbutton.Layer.CornerRadius = 5f;
                View.AddSubview(cancelbutton);
                cancelbutton.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

                var urlstr = string.Empty;
                var item = ExtensionContext.InputItems[0];
                NSItemProvider prov = null;
                if (item != null)  
                   prov = item.Attachments[0];
                if (prov != null)
                {
                    prov.LoadItem(UTType.URL, null, (NSObject url, NSError error) =>
                    {
                        if (url == null)
                            return;
                        NSUrl newUrl = (NSUrl)url;
                        var newUrl2 = newUrl.ToString();
                        var resultSotap = Regex.Match(MetaScraper.GetMetaDataFromUrl(newUrl2).Title, @"\d+").Value;



                    //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    db = new SQLiteConnection(ShareLibs.GetDatabasePath());
                        var PhimmoiEx = new movDatabase();
                        InvokeOnMainThread(() =>
                        {
                        
                            tenphimTextViewEx.Text = MetaScraper.GetMetaDataFromUrl(newUrl2).Title;
                            sotapTextViewEx.Text = resultSotap;
                            imageViewEx.Image = FromUrl(MetaScraper.GetMetaDataFromUrl(newUrl2).ImageUrl);
                            addbutton.TouchUpInside += (sender, e) =>
                            {
                                PhimmoiEx.Tenphim = tenphimTextViewEx.Text;
                                try
                                {
                                    PhimmoiEx.img = StoreImageEx(tenphimTextViewEx.Text);
                                }
                                catch
                                {

                                }
                                try
                                {
                                    PhimmoiEx.Sotap = Convert.ToInt32(sotapTextViewEx.Text);
                                }
                                catch
                                {

                                }
                                try
                                {
                                    PhimmoiEx.min = Convert.ToInt32(sophutTextViewEx.Text);
                                }
                                catch
                                {

                                }
                                PhimmoiEx.link = newUrl2;
                                db.Insert(PhimmoiEx);

                                var alert = UIAlertController.Create("Movie Mark", "Added", UIAlertControllerStyle.Alert);
                                PresentViewController(alert, true, () =>
                                {
                                    DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 500000000), () =>
                                    {
                                    // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
                                    ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
                                    });
                                });

                            };
                            cancelbutton.TouchUpInside += (sender, e) =>
                            {
                                var alert = UIAlertController.Create("Movie Mark", "Canceled", UIAlertControllerStyle.Alert);
                                PresentViewController(alert, true, () =>
                                {
                                    DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 500000000), () =>
                                    {
                                    // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
                                    ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
                                    });
                                });
                            };

                        });

                    });

                }
            
          

        }

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        public string StoreImageEx(string filename2)
        {
            //string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var FileManager = new NSFileManager();
            var appGroupContainer = FileManager.GetContainerUrl("group.pro.ozzies.moviebookmarker");
            var appGroupContainerPath = appGroupContainer.Path;
            var fullpath2 = Path.Combine(appGroupContainerPath, filename2 + ".jpg");

            //var img = UIImage.FromFile(filename);
            var img2 = imageViewEx.Image;
            NSData image2 = img2.AsJPEG();
            NSError err = null;

            image2.Save(fullpath2, false, out err);
            //imgProfile.Image = UIImage.FromFile(fileName);
            return filename2 + ".jpg";
        }
    }
}
