using System;
using System.Collections.Generic;
using System.Linq;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Acciona.Presentation.UI.Features.WorkingCenter;
using BaseIOS.UI;
using CoreGraphics;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.WorkingCenter
{
    public partial class WorkingCenterViewController : BaseViewController<WorkingCenterPresenter>, WorkingCenterUI
    {
        private Ficha ficha;
        private string other;
        private IEnumerable<Location> locations;
        private List<string> paises;
        private Location selected;
        private List<string> ciudades;

        public WorkingCenterViewController(Ficha ficha) : base("WorkingCenterViewController", null)
        {
            this.ficha = ficha;
        }

        protected override void AssingViews()
        {
            BackButton.TouchUpInside += (o, e) => presenter.BackClicked();
            ModifyButton.TouchUpInside += ModifyButton_TouchUpInside;

            PaisListTextField.ItemChanged += (o, pais) =>
            {
                if (pais.Equals(other))
                {
                    CiudadListTextField.SetListableObjects(new List<string>() { other });
                    CenterListTextField.SetListableObjects(new List<Location>() { new Location() { Name = other, Pais = other, Ciudad = other, IdLocation = -1 } });
                }
                else
                {
                    var filtered = locations.Where(x => x.Pais.Equals(pais)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais = pais, Ciudad = other, IdLocation = -1 });
                    CenterListTextField.SetListableObjects(filtered);
                    ciudades = locations.Where(x => x.Pais.Equals(pais)).Select(x => x.Ciudad).Distinct().ToList();
                    ciudades.Sort();
                    ciudades.Insert(0, other);
                    CiudadListTextField.SetListableObjects(ciudades);
                }
            };
            CiudadListTextField.ItemChanged += (o, ciudad) =>
            {
                if (ciudad.Equals(other))
                {
                    var filtered = locations.Where(x => x.Pais.Equals(PaisListTextField.Text)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais = PaisListTextField.Text, Ciudad = other, IdLocation = -1 });
                    CenterListTextField.SetListableObjects(filtered);
                }
                else
                {
                    var filtered = locations.Where(x => x.Pais.Equals(PaisListTextField.Text) && x.Ciudad.Equals(ciudad)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais = PaisListTextField.Text, Ciudad = ciudad, IdLocation = -1 });
                    CenterListTextField.SetListableObjects(filtered);
                }
            };
            CenterListTextField.ItemChanged += (o, loc) =>
            {
                var l = loc as Location;
                int index = ciudades.IndexOf(l.Ciudad);
                CiudadListTextField.SetSelectionIndex(index);
                if (index >= 0 && l.IdLocation == -1)
                    l.Ciudad = ciudades[index];

            };


            styleView();
            applyTraslations();
            other= AppDelegate.LanguageBundle.GetLocalizedString("center_other");
        }

        private void ModifyButton_TouchUpInside(object sender, EventArgs e)
        {
            var loc = CenterListTextField.GetSelection() as Location;
            presenter.ModifyClicked(loc);
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.SetFicha(ficha);
        }

        private void applyTraslations()
        {
            TitleViewLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_title");

            Label1.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_country");
            Label2.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_city");
            Label3.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_center");
            mandatoryLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_mandatory");
            ModifyButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("center_modify") , UIControlState.Normal);
        }

        private void styleView()
        {
            TitleViewLabel.Font = Styles.SetHelveticaBoldFont(17);
            Label1.Font = Styles.SetHelveticaFont(18);
            Label2.Font = Styles.SetHelveticaFont(18);
            Label3.Font = Styles.SetHelveticaFont(18);
            Label1.TextColor = UIColor.Black;
            Label2.TextColor = UIColor.Black;
            Label3.TextColor = UIColor.Black;
            ModifyButton.Layer.CornerRadius = 4;            
        }

        public void SetSelectedLocation(Location location)
        {
            this.selected = location;
            if (selected != null)
            {
                PaisListTextField.Text = location?.Pais;
                CiudadListTextField.Text = location?.Ciudad;
                CenterListTextField.Text = location?.Name;
            }
            else
            {
                //buttonBack.Visibility = ViewStates.Invisible;
            }
        }

        public void SetLocations(IEnumerable<Location> locations)
        {
            this.locations = locations;
            paises = locations.Select(x => x.Pais).Distinct().ToList();
            paises.Sort();
            paises.Insert(0, other);            
            PaisListTextField.SetListableObjects(paises);
            if (selected == null)
                PaisListTextField.ForceInvokeEvent();
            else
            {
                int indexPais = paises.IndexOf(selected.Pais);
                PaisListTextField.SetSelectionIndex(indexPais);
                ciudades = locations.Where(x => x.Pais.Equals(selected.Pais)).Select(x => x.Ciudad).Distinct().ToList();
                ciudades.Sort();
                ciudades.Insert(0, other);
                CiudadListTextField.SetListableObjects(ciudades);
                var index = ciudades.IndexOf(selected.Ciudad);
                CiudadListTextField.SetSelectionIndex(index);
                var filtered = locations.Where(x => x.Pais.Equals(selected.Pais) && x.Ciudad.Equals(selected.Ciudad)).OrderBy(x => x.Name).ToList();
                filtered.Insert(0, new Location() { Name = other, Pais = paises[indexPais], Ciudad = ciudades[index], IdLocation = -1 });
                var s = filtered.Where(x => x.IdLocation == selected.IdLocation).First();
                index = filtered.IndexOf(s);
                CenterListTextField.SetListableObjects(filtered);
                CenterListTextField.SetSelectionIndex(index);
            }
        }
    }
}

