using AutoMapper;
using ServerManagement.Model.Entity;
using ServerManagement.VML;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ServerManagement.Model
{
    public class ServerModel : INotifyPropertyChanged
    {
        public ServerModel()
        {
            mCreateCommand = new AsyncDelegateCommand(CreateServerAsync, CanCreateServer);
        }

        #region Attributes + getset
        private bool mIsSelected;
        private int mId;
        private string mName;
        private string mHostName;
        private string mUsername;
        private string mPassword;
        private string mFunctionality;
        private string mProject;
        private string mNameTag;
        private bool mActive;

        private string mIpMng;
        private string mIpWan;
        private string mIpLan;
        private string mIpDB;
        private string mMacMng;
        private string mMacWan;
        private string mMacLan;
        private string mMacDB;

        private string mStatus;
        private Brush mColor;

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                if (value != this.mName)
                {
                    mName = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string HostName
        {
            get
            {
                return this.mHostName;
            }
            set
            {
                if (value != this.mHostName)
                {
                    mHostName = value;
                    RaisePropertyChanged("HostName");
                }
            }
        }
        public string Username
        {
            get
            {
                return this.mUsername;
            }
            set
            {
                if (value != this.mUsername)
                {
                    mUsername = value;
                    RaisePropertyChanged("Username");
                }
            }
        }
        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (value != this.mPassword)
                {
                    mPassword = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        public string Project
        {
            get
            {
                return this.mProject;
            }
            set
            {
                if (value != this.mProject)
                {
                    mProject = value;
                    RaisePropertyChanged("Project");
                }
            }
        }
        public string NameTag
        {
            get
            {
                return this.mNameTag;
            }
            set
            {
                if (value != this.mNameTag)
                {
                    mNameTag = value;
                    RaisePropertyChanged("NameTag");
                }
            }
        }
        public string IpMng
        {
            get
            {
                return mIpMng;
            }
            set
            {
                if (value != this.mIpMng)
                {
                    mIpMng = value;
                    RaisePropertyChanged("IpMng");
                }
            }
        }
        public string IpWan
        {
            get
            {
                return mIpWan;
            }
            set
            {
                if (value != this.mIpWan)
                {
                    mIpWan = value;
                    RaisePropertyChanged("IpWan");
                }
            }
        }
        public string IpLan
        {
            get
            {
                return mIpLan;
            }
            set
            {
                if (value != this.mIpLan)
                {
                    mIpLan = value;
                    RaisePropertyChanged("IpLan");
                }
            }
        }
        public string IpDB
        {
            get
            {
                return mIpDB;
            }
            set
            {
                if (value != this.mIpDB)
                {
                    mIpDB = value;
                    RaisePropertyChanged("IpDB");
                }
            }
        }
        public string MacMng
        {
            get
            {
                return mMacMng;
            }
            set
            {
                if (value != this.mMacMng)
                {
                    mMacMng = value;
                    RaisePropertyChanged("MacMng");
                }
            }
        }
        public string MacWan
        {
            get
            {
                return mMacWan;
            }
            set
            {
                if (value != this.mMacWan)
                {
                    mMacWan = value;
                    RaisePropertyChanged("MacWan");
                }
            }
        }
        public string MacLan
        {
            get
            {
                return mMacLan;
            }
            set
            {
                if (value != this.mMacLan)
                {
                    mMacLan = value;
                    RaisePropertyChanged("MacLan");
                }
            }
        }
        public string MacDB
        {
            get
            {
                return mMacDB;
            }
            set
            {
                if (value != this.mMacDB)
                {
                    mMacDB = value;
                    RaisePropertyChanged("MacDB");
                }
            }
        }
        public string Functionality
        {
            get
            {
                return this.mFunctionality;
            }
            set
            {
                if (value != this.mFunctionality)
                {
                    mFunctionality = value;
                    RaisePropertyChanged("Functionality");
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                mIsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public int Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
                RaisePropertyChanged("Id");
            }
        }
        public bool Active
        {
            get
            {
                return this.mActive;
            }
            set
            {
                if (value != this.mActive)
                {
                    mActive = value;
                    RaisePropertyChanged("Active");
                }
            }
        }

        public string Status
        {
            get
            {
                return mStatus;
            }
            set
            {
                mStatus = value;
                RaisePropertyChanged("Status");
            }
        }
        public Brush StatusColor
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
                RaisePropertyChanged("StatusColor");
            }
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }
        public bool IsAuthorized
        {

            get { return (Thread.CurrentPrincipal.IsInRole("Admin") || Thread.CurrentPrincipal.IsInRole("Writer")); }
        }
        #endregion

        #region Commands
        private readonly AsyncDelegateCommand mCreateCommand;
        public AsyncDelegateCommand AsyncCreateServerCommand
        {
            get
            {
                return mCreateCommand;
            }
        }

        private Task CreateServerAsync()
        {
            return Task.Run(new Action(() =>
            {
                using (ServerManagementEntities db = new ServerManagementEntities())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);

                            var entity = db.Servers.Find(Id);
                            if (entity == null)//create server
                            {
                                entity = new Server
                                {
                                    Active = true,
                                    Name = this.Name,
                                    HostName = this.HostName,
                                    Username = this.Username,
                                    Password = c.Encrypt(this.Password),
                                    Project = this.Project,
                                    NameTag = this.NameTag,
                                    Functionality = this.Functionality,
                                };

                                db.Servers.Add(entity);
                                db.SaveChanges();

                                Utils.RecordActivityAsync(ActivityType.Create, "Servers", DateTime.Now, entity.Id, Utils.GetCurrentUser().UserId);

                                StatusColor = Brushes.Green;
                                Status = "New Server Created !";
                            }
                            else//update server
                            {
                                var oldIps = db.IPs.Where(q => q.Active && q.ServerId == this.Id);
                                foreach (var ip in oldIps)
                                {
                                    ip.Active = false;
                                    db.Entry(ip).State = System.Data.Entity.EntityState.Modified;
                                }

                                var updatedServer = new Server
                                {
                                    Id = this.Id,
                                    Active = true,
                                    Name = this.Name,
                                    HostName = this.HostName,
                                    Username = this.Username,
                                    Password = c.Encrypt(this.Password),
                                    Project = this.Project,
                                    NameTag = this.NameTag,
                                    Functionality = this.Functionality,
                                };
                                db.Entry(entity).CurrentValues.SetValues(updatedServer);
                                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                                Utils.RecordActivityAsync(ActivityType.Update, "Servers", DateTime.Now, entity.Id, Utils.GetCurrentUser().UserId);

                                StatusColor = Brushes.Green;
                                Status = "Updated !";
                            }

                            if (!string.IsNullOrEmpty(this.IpMng))
                            {
                                var ip = new IP
                                {
                                    Active = true,
                                    IPAddress = this.IpMng,
                                    ServerId = entity.Id,
                                    Type = 1,
                                };
                                if (!string.IsNullOrEmpty(this.MacMng))
                                {
                                    var macAddress = new MacAddress
                                    {
                                        Active = true,
                                        MacAddress1 = this.MacMng
                                    };
                                    db.MacAddresses.Add(macAddress);
                                    db.SaveChanges();
                                    ip.MacAddressId = macAddress.Id;
                                }

                                db.IPs.Add(ip);
                                db.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(this.IpWan))
                            {
                                var ip = new IP
                                {
                                    Active = true,
                                    IPAddress = this.IpWan,
                                    ServerId = entity.Id,
                                    Type = 2,
                                };
                                if (!string.IsNullOrEmpty(this.MacWan))
                                {
                                    var macAddress = new MacAddress
                                    {
                                        Active = true,
                                        MacAddress1 = this.MacWan
                                    };
                                    db.MacAddresses.Add(macAddress);
                                    db.SaveChanges();
                                    ip.MacAddressId = macAddress.Id;
                                }

                                db.IPs.Add(ip);
                                db.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(this.IpLan))
                            {
                                var ip = new IP
                                {
                                    Active = true,
                                    IPAddress = this.IpLan,
                                    ServerId = entity.Id,
                                    Type = 3,
                                };
                                if (!string.IsNullOrEmpty(this.MacLan))
                                {
                                    var macAddress = new MacAddress
                                    {
                                        Active = true,
                                        MacAddress1 = this.MacLan
                                    };
                                    db.MacAddresses.Add(macAddress);
                                    db.SaveChanges();
                                    ip.MacAddressId = macAddress.Id;
                                }

                                db.IPs.Add(ip);
                                db.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(this.IpDB))
                            {
                                var ip = new IP
                                {
                                    Active = true,
                                    IPAddress = this.IpDB,
                                    ServerId = entity.Id,
                                    Type = 4,
                                };
                                if (!string.IsNullOrEmpty(this.MacDB))
                                {
                                    var macAddress = new MacAddress
                                    {
                                        Active = true,
                                        MacAddress1 = this.MacDB
                                    };
                                    db.MacAddresses.Add(macAddress);
                                    db.SaveChanges();
                                    ip.MacAddressId = macAddress.Id;
                                }

                                db.IPs.Add(ip);
                                db.SaveChanges();
                            }

                            db.SaveChanges();
                            trans.Commit();
                        }
                        catch (System.Exception ex)
                        {
                            trans.Rollback();
                            StatusColor = Brushes.Red;
                            Status = "Error! Please check and try again";
                            VML.Utils.WriteLog(ex.Message);
                        }
                    }
                }
            }));
        }
        private bool CanCreateServer()
        {
            return IsAuthorized && IsAuthenticated;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
