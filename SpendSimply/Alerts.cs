using System;
using UIKit;
using Foundation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SpendSimply
{
    public class Alerts
    {
        public Alerts()
        {
        }

        public static void GenerateNewItemTitleAlert(MainView controller)
        {
            var addItemAlert = UIAlertController.Create("Item Name", "Enter a name for the new item.", UIAlertControllerStyle.Alert);
            UITextField field = null;

            var itemNameFilledIn = UIAlertAction.Create("Next", UIAlertActionStyle.Default, action =>
            {
                var newItemTitleField = addItemAlert.TextFields[0];
                GenerateNewItemValueAlert(newItemTitleField.Text, controller);
            });
            itemNameFilledIn.Enabled = false;

            addItemAlert.AddTextField((textField) =>
                {
                    field = textField;
                    field.AutocorrectionType = UITextAutocorrectionType.No;
                    field.KeyboardType = UIKeyboardType.Default;
                    field.ReturnKeyType = UIReturnKeyType.Done;
                    field.ClearButtonMode = UITextFieldViewMode.WhileEditing;
                    field.AddTarget((object sender, EventArgs e) =>
                    {
                        if ((((UITextField)sender).Text.Length > 0) && ((UITextField)sender).Text.Length < 30)
                        {
                            itemNameFilledIn.Enabled = true;
                        }
                        else
                        {
                            itemNameFilledIn.Enabled = false;
                        }
                    }, UIControlEvent.EditingChanged);
                });

            addItemAlert.AddAction(itemNameFilledIn);
            addItemAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            controller.PresentViewController(addItemAlert, true, null);
        }

        public static void GenerateNewItemValueAlert(string itemTitle, MainView controller)
        {
            var addValueAlert = UIAlertController.Create("Item Value", "Enter a value for the new item.", UIAlertControllerStyle.Alert);
            UITextField field = null;

            var itemValueFilledIn = UIAlertAction.Create("Create", UIAlertActionStyle.Default, action =>
                {
                    AddNewItem(itemTitle, decimal.Parse(addValueAlert.TextFields[0].Text));
                    //Starts a new non-ui thread (backgrounded thread) to complete the task 
                    //System.Threading.Tasks.Task.Run(() =>
                    //{
                    MainView.collectionView.RemoveFromSuperview();
                    MainView.LoadCollectionView();
                });
            itemValueFilledIn.Enabled = false;

            addValueAlert.AddTextField((textField) =>
                {
                    field = textField;
                    field.AutocorrectionType = UITextAutocorrectionType.No;
                    field.KeyboardType = UIKeyboardType.DecimalPad;
                    field.ReturnKeyType = UIReturnKeyType.Done;
                    field.ClearButtonMode = UITextFieldViewMode.WhileEditing;
                    field.AddTarget((object sender, EventArgs e) =>
                    {
                        if ((((UITextField)sender).Text.Length > 0) && ((UITextField)sender).Text.Length < 13)
                        {
                            var rgx = @"^\d*\.?\d{0,2}$";
                            var match = Regex.Match(((UITextField)sender).Text, rgx);
                            if (match.Success)
                            {
                                itemValueFilledIn.Enabled = true;
                            }
                            else
                            {
                                itemValueFilledIn.Enabled = false;
                                //return;
                            }
                        }
                        else
                        {
                            itemValueFilledIn.Enabled = false;
                        }
                    }, UIControlEvent.EditingChanged);
                });

            addValueAlert.AddAction(itemValueFilledIn);
            addValueAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            controller.PresentViewController(addValueAlert, true, null);
        }

        public static void AddNewItem(string itemTitle, decimal itemValue)
        {
            var newItem = new DBofItems();
            newItem.ItemName = itemTitle;
            newItem.ItemPrice = Math.Round(itemValue, 2);
            AppDelegate.itemsToTrack.AddItem(newItem);
        }

        public static void PresentDeleteOptions(MainView controller, DBofItems itemToResetOrDelete, UIButton deleteButton, UILabel text)
        {
            var deleteOptionsAlert = UIAlertController.Create("Reset or Delete?", null, UIAlertControllerStyle.ActionSheet);
            deleteOptionsAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action =>
            {
                deleteButton.RemoveFromSuperview();
                text.UserInteractionEnabled = true;
            }));
            deleteOptionsAlert.AddAction(UIAlertAction.Create("Reset", UIAlertActionStyle.Default, action =>
            {
                AppDelegate.itemsToTrack.ResetAllTapEventsOfItem(itemToResetOrDelete);
                deleteButton.RemoveFromSuperview();
                text.UserInteractionEnabled = true;
                if (text.Text != itemToResetOrDelete.ItemName)
                {
                    ItemCollectionViewCell.UpdateDetails(text, itemToResetOrDelete);
                }
                MainView.UpdateCostOfAllItems();
            }));
            deleteOptionsAlert.AddAction(UIAlertAction.Create("Delete", UIAlertActionStyle.Default, action =>
            {
                AppDelegate.itemsToTrack.RemoveItem(itemToResetOrDelete);
                MainView.collectionView.RemoveFromSuperview();
                MainView.LoadCollectionView();
                MainView.UpdateCostOfAllItems();
            }));
            controller.PresentViewController(deleteOptionsAlert, true, null);
        }

        public static void ResetAllTrackedItems(MainView controller)
        {
            var confirmResetAlert = UIAlertController.Create("Reset tracking for all items?", null, UIAlertControllerStyle.Alert);
            confirmResetAlert.AddAction(UIAlertAction.Create("Reset", UIAlertActionStyle.Default, action =>
            {
                var allItems = AppDelegate.itemsToTrack.GetAllItemProperties();
                foreach (var item in allItems)
                {
                    AppDelegate.itemsToTrack.ResetAllTapEventsOfItem(item);
                }
                MainView.collectionView.RemoveFromSuperview();
                MainView.LoadCollectionView();
                MainView.UpdateCostOfAllItems();
            }));
            confirmResetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            controller.PresentViewController(confirmResetAlert, true, null);
        }
    }
}