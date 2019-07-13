﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EGIS.Projections;

namespace EGIS.Controls
{
    public partial class CRSSelectionControl : UserControl
    {
        private ICRSFactory crsFactory;
        private ICRS selectedCRS = null;

        public CRSSelectionControl()
        {
            InitializeComponent();
        }


        #region public properties and methods

        public void LoadCoordinateSystems(ICRSFactory crsFactory)
        {
            this.crsFactory = crsFactory;

            LoadCoordinateSystems();
        }

        public ICRS SelectedCRS
        {
            get
            {
                return this.selectedCRS;
            }
            set
            {
                this.selectedCRS = value;
                UpdateSelectedCRS();
            }
        }

       
        #endregion

        private void LoadCoordinateSystems()
        {
            this.cbSelectedCRS.Items.Clear();
            if (this.crsFactory == null) return;

            if (this.rbGeographic.Checked)
            {
                this.cbSelectedCRS.Items.AddRange(crsFactory.GeographicCoordinateSystems.OrderBy(o => o.Name).ToArray());
            }
            else if (this.rbProjected.Checked)
            {
                this.cbSelectedCRS.Items.AddRange(crsFactory.ProjectedCoordinateSystems.OrderBy(o => o.Name).ToArray());
            }
            if (this.cbSelectedCRS.Items.Count > 0)
            {
                this.cbSelectedCRS.SelectedIndex = 0;
            }
        }

        private void UpdateSelectedCRS()
        {
            if (this.selectedCRS == null)
            {
                this.txtWKT.Text = "";
            }
            else
            {
                this.txtWKT.Text = this.selectedCRS.WKT;
                int code = 0;
                int.TryParse(this.selectedCRS.Id, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out code);
                this.nudEPGS.Value = code;
                
            }
        }

        private void cbSelectedCRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelectedCRS.SelectedIndex >= 0)
            {
                this.selectedCRS = cbSelectedCRS.SelectedItem as ICRS;
                UpdateSelectedCRS();
            }
        }

        private void btnFindEPGS_Click(object sender, EventArgs e)
        {
            this.selectedCRS = FindCRSByEpgsCode((int)nudEPGS.Value);
            UpdateSelectedCRS();
        }

        private ICRS FindCRSByEpgsCode(int code)
        {
            if(this.crsFactory == null) return null;

            return this.crsFactory.GetCRSById(code);

        }

        private void rbGeographic_CheckedChanged(object sender, EventArgs e)
        {
            LoadCoordinateSystems();
        }

        private void rbProjected_CheckedChanged(object sender, EventArgs e)
        {
            LoadCoordinateSystems();
        }

    }
}