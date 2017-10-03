using AutoMapper;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for NewServer.xaml
    /// </summary>
    public partial class NewServer : UserControl
    {
        public ServerModel newServer = new ServerModel();
        private readonly ServerManagementEntities db = new ServerManagementEntities();

        public NewServer()
        {
            InitializeComponent();
            this.DataContext = newServer;
        }

        private async void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);

                ServerModel updatedServer = ((FrameworkElement)sender).DataContext as ServerModel;
                var entity = db.Servers.Find(updatedServer.Id);
                if (entity == null)//create server
                {
                    newServer.Password = c.Encrypt(newServer.Password);
                    newServer.Active = true;

                    entity = Mapper.Map<Model.Entity.Server>(newServer);
                    db.Servers.Add(entity);
                    db.SaveChanges();

                    foreach (var IpMac in Ips.Children)
                    {
                        if (IpMac is IpMac)
                        {
                            string ip = ((IpMac as IpMac).FindName("txtIP") as TextBox).Text;
                            string mac = ((IpMac as IpMac).FindName("txtMac") as TextBox).Text;

                            var newMac = new MacAddress
                            {
                                Active = true,
                                MacAddress1 = mac,
                            };
                            db.MacAddresses.Add(newMac);
                            db.SaveChanges();

                            db.IPs.Add(new IP
                            {
                                Active = true,
                                IPAddress = ip,
                                MacAddressId = newMac.Id,
                                ServerId = entity.Id
                            });
                        }
                    }
                }
                else//update server
                {
                    var oldIps = db.IPs.Where(q => q.Active && q.ServerId == updatedServer.Id);
                    foreach (var ip in oldIps)
                    {
                        ip.Active = false;
                        db.Entry(ip).State = System.Data.Entity.EntityState.Modified;
                    }

                    foreach (var IpMac in Ips.Children)
                    {
                        if (IpMac is IpMac)
                        {
                            string ip = ((IpMac as IpMac).FindName("txtIP") as TextBox).Text;
                            string mac = ((IpMac as IpMac).FindName("txtMac") as TextBox).Text;

                            var newMac = new MacAddress
                            {
                                Active = true,
                                MacAddress1 = mac,
                            };
                            db.MacAddresses.Add(newMac);
                            db.SaveChanges();

                            db.IPs.Add(new IP
                            {
                                Active = true,
                                IPAddress = ip,
                                MacAddressId = newMac.Id,
                                ServerId = entity.Id
                            });
                        }
                    }
                    updatedServer.Password = c.Encrypt(updatedServer.Password);
                    db.Entry(entity).CurrentValues.SetValues(Mapper.Map<Model.Entity.Server>(updatedServer));
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
                btnSave.Content = "Saved";
            }
            catch (System.Exception ex)
            {
                Utils.WriteLog(ex.Message);
            }
        }

        private void BtnAddIp_Click(object sender, RoutedEventArgs e)
        {
            Ips.Children.Add(new IpMac());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ServerModel updatedServer = ((FrameworkElement)sender).DataContext as ServerModel;
            if (updatedServer.Id != 0)
            {
                foreach (var ip in updatedServer.IpAddresses)
                {
                    Ips.Children.Add(new IpMac
                    {
                        DataContext = ip,
                    });
                }
            }
        }
    }
}
