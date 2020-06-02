using DCIM.Data.Entities;
using DCIM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCIM.Services
{
    public class DCIMServices
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly DCIMDbContext _dbContext;
        public DCIMServices(IWebHostEnvironment hostingEnvironment, DCIMDbContext dbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbContext = dbContext;
        }

        public List<Rack> GetRacks(string type)
        {
            int i = type == "D" ? 1 : 0;
            return _dbContext.Rack.ToList();
        }
        public List<RackUnit> GetRackUnit(int rackId)
        {
            var serverUnitId = _dbContext.Server.Select(e => e.RackUnitId).ToList();
            var rec = (from r in _dbContext.RackUnit
                       where !serverUnitId.Contains(r.Id) && r.RackId == rackId
                       select new RackUnit()
                       {
                           Id = r.Id,
                           FromUnit = r.FromUnit
                       }).Distinct().ToList();
            return rec;
        }
        public List<Server> GetServers(int rackId, string type)
        {
            int i = type == "D" ? 1 : 0;
            return _dbContext.Server.Where(e => e.RackId == rackId).ToList();
        }
        public List<Server> GetNotMappedServers()
        {
            return _dbContext.Server.Where(e => e.RackId == 0 && e.RackUnitId == 0).ToList();
        }
        public List<ServerSlotDtl> GetServerSlots(int serverId)
        {
            return (from s in _dbContext.ServerSlotDtl
                    where s.ServerId == serverId
                    select s).ToList();

        }
        public List<ServerPortDtl> GetServerPorts(int serverId, int slotId)
        {
            return (from s in _dbContext.ServerPortDtl
                    where s.ServerId == serverId && s.ServerSlotDtlId == slotId
                    select s).ToList();
        }

        public DestinationIds GetDestinationIds(int portId)
        {
            DestinationIds destinationIds = new DestinationIds();
            var rec = _dbContext.Connections.Where(e => e.SourcePortId == portId).FirstOrDefault();
            if (rec != null)
            {
                destinationIds.DPortId = rec.DestinationPortId;
                destinationIds.DRackId = rec.DestinationRackId;
                destinationIds.DServerId = rec.DestinationServerId;
                destinationIds.DSlotId = rec.DestinationSlotId;
            }
            return destinationIds;
        }
        public string MakePath(DCIMModel info)
        {
            string msg = string.Empty;
            var rec = _dbContext.Connections.Where(e =>
            e.SourcePortId == info.SPort
            ).FirstOrDefault();
            if (rec == null)
            {
                Connections connections = new Connections()
                {
                    DestinationPortId = info.DPort,
                    DestinationRackId = info.DRack,
                    DestinationServerId = info.DServer,
                    SourcePortId = info.SPort,
                    SourceRackId = info.SRack,
                    SourceServerId = info.SServer,
                    DestinationSlotId = info.DSlot,
                    SourceSlotId = info.SSlot

                };
                _dbContext.Connections.Add(connections);
                _dbContext.SaveChanges();
            }
            else
            {
                msg = "Selected source port is already connected to different destination port. Please disconenct the previous connection and Try again.";
            }
            return msg;
        }
        public string DisconnectPath(ConnectionList info)
        {
            string msg = string.Empty;
            var rec = _dbContext.Connections.Where(e =>
            e.SourcePortId == info.SPortId && e.SourceRackId == info.SRackId && e.SourceServerId == info.SServerId && e.SourceSlotId == info.SSlotId
            && e.DestinationPortId == info.DPortId && e.DestinationRackId == info.DRackId && e.DestinationServerId == e.DestinationServerId && e.DestinationSlotId == info.DSlotId
            ).FirstOrDefault();
            if (rec != null)
            {
                _dbContext.Connections.Remove(rec);
                _dbContext.SaveChanges();
            }
            return msg;
        }
        public List<ConnectionList> GetConnectionLists()
        {
            var rec = (from c in _dbContext.Connections
                       join dp in _dbContext.ServerPortDtl on c.DestinationPortId equals dp.Id
                       join dsp in _dbContext.ServerSlotDtl on c.DestinationSlotId equals dsp.Id
                       join ds in _dbContext.Server on c.DestinationServerId equals ds.Id
                       join dr in _dbContext.Rack on c.DestinationRackId equals dr.Id

                       join sp in _dbContext.ServerPortDtl on c.SourcePortId equals sp.Id
                       join ssp in _dbContext.ServerSlotDtl on c.DestinationSlotId equals ssp.Id
                       join ss in _dbContext.Server on c.SourceServerId equals ss.Id
                       join sr in _dbContext.Rack on c.SourceRackId equals sr.Id
                       select new ConnectionList()
                       {
                           DPort = dp.Port,
                           DPortId = c.DestinationPortId,
                           DSlotId = c.SourceSlotId,
                           DSlot = dsp.SlotName,
                           DRack = dr.Rack1,
                           DRackId = c.DestinationRackId,
                           DServer = ds.Server1,
                           DServerId = c.DestinationServerId,
                           SPort = sp.Port,
                           SPortId = c.SourcePortId,
                           SSlotId = c.SourceSlotId,
                           SSlot = ssp.SlotName,
                           SRack = sr.Rack1,
                           SRackId = c.SourceRackId,
                           SServer = ss.Server1,
                           SServerId = c.SourceServerId

                       }).ToList();

            return rec;
        }


        public string GetRackImage(int rackId)
        {
            string base64String = string.Empty;
            var rackRec = _dbContext.Rack.Where(e => e.Id == rackId).FirstOrDefault();
            if (rackRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + rackRec.ImagePath;
                base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(ImagePath));
            }
            return base64String;
        }
        public string GetRackUnitImage(RackServerMap info)
        {
            string base64String = string.Empty;
            int rackId = info.Rack;
            int rackUnitId = info.RackUnit;
            var rackRec = _dbContext.Rack.Where(e => e.Id == rackId).FirstOrDefault();
            if (rackRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + rackRec.ImagePath;
                var fileName = Path.GetFileName(ImagePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ImagePath);
                var rackUnit = _dbContext.RackUnit.Where(e => e.Id == rackUnitId).ToList();
                if (rackUnit.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        {
                            foreach (var p in rackUnit)
                            {
                                Pen pen = new Pen(Color.Red, 5);
                                if (p.Id == rackUnitId)
                                {
                                    pen = new Pen(Color.Yellow, 5);
                                }

                                Rectangle rect = new Rectangle(p.Xcoordinate, p.Ycoordinate, p.RectangleXcoordinate, p.RectangleYcoordinate);
                                using (Graphics G = Graphics.FromImage(bmp))
                                {
                                    G.DrawRectangle(pen, rect);
                                }


                            }
                            bmp.Save(@"C:\temp\" + fileName, ImageFormat.Png); // ImageFormat.Jpeg, etc
                        }
                        string path = @"c:\temp\" + fileName;
                        base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                        //System.IO.File.Delete(path);
                    }
                }

            }
            return base64String;
        }
        public string GetNotMappedServerImage(RackServerMap info)
        {
            string base64String = string.Empty;
            var rackRec = _dbContext.Server.Where(e => e.Id == info.Server).FirstOrDefault();
            if (rackRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + rackRec.ImagePath;
                base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(ImagePath));
            }
            return base64String;
        }
        public string GetServerImage(DCIMModel info, bool IsDRack)
        {
            string base64String = string.Empty;
            int rackId = IsDRack ? info.DRack : info.SRack;
            int serverId = IsDRack ? info.DServer : info.SServer;
            var rackRec = _dbContext.Rack.Where(e => e.Id == rackId).FirstOrDefault();
            var serverRec = _dbContext.Server.Where(e => e.Id == serverId).FirstOrDefault();
            if (rackRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + rackRec.ImagePath;
                var fileName = Path.GetFileName(ImagePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ImagePath);
                var server = _dbContext.Server.Where(e => e.Id == serverRec.Id).ToList();
                if (server.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        for (int i = 0; i < bmp.Width; i++)
                        {
                            for (int j = 0; j < bmp.Height; j++)
                            {
                                foreach (var p in server)
                                {
                                    Pen pen = new Pen(Color.Red, 5);
                                    if (p.Id == serverId)
                                    {
                                        pen = new Pen(Color.Yellow, 5);
                                    }

                                    Rectangle rect = new Rectangle(p.Xcoordinate, p.Ycoordinate, p.RectangleXcoordinate, p.RectangleYcoordinate);
                                    using (Graphics G = Graphics.FromImage(bmp))
                                    {
                                        G.DrawRectangle(pen, rect);
                                    }

                                    i++;
                                }
                            }

                        }
                        bmp.Save(@"C:\temp\" + fileName, ImageFormat.Png); // ImageFormat.Jpeg, etc
                    }
                    string path = @"c:\temp\" + fileName;
                    base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    System.IO.File.Delete(path);
                }
            }
            return base64String;
        }
        public string GetServerSlotImage(DCIMModel info, bool isDport)
        {
            string base64String = string.Empty;
            int serveId = isDport ? info.DServer : info.SServer;
            int slotId = isDport ? info.DSlot : info.SSlot;
            var serverRec = _dbContext.Server.Where(e => e.Id == serveId).FirstOrDefault();
            if (serverRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + serverRec.ImagePath;
                var fileName = Path.GetFileName(ImagePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ImagePath);
                var slots = _dbContext.ServerSlotDtl.Where(e => e.ServerId == serverRec.Id).ToList();
                if (slots.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        for (int i = 0; i < bmp.Width; i++)
                        {
                            for (int j = 0; j < bmp.Height; j++)
                            {
                                foreach (var p in slots)
                                {
                                    Pen pen = new Pen(Color.Red, 5);
                                    if (p.Id == slotId)
                                    {
                                        pen = new Pen(Color.Yellow, 5);
                                    }

                                    Rectangle rect = new Rectangle(p.Xcoordinate, p.Ycoordinate, p.RectangleXcoordinate, p.RectangleYcoordinate);
                                    using (Graphics G = Graphics.FromImage(bmp))
                                    {
                                        G.DrawRectangle(pen, rect);
                                    }

                                    i++;
                                }
                            }

                        }
                        bmp.Save(@"C:\temp\" + fileName, ImageFormat.Png); // ImageFormat.Jpeg, etc
                    }
                    string path = @"c:\temp\" + fileName;
                    base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    System.IO.File.Delete(path);
                }
            }
            return base64String;
        }
        public string GetServerPortImage(DCIMModel info, bool isDport)
        {
            string base64String = string.Empty;
            int slotId = isDport ? info.DServer : info.SServer;
            int portId = isDport ? info.DPort : info.SPort;
            var slotRec = _dbContext.Server.Where(e => e.Id == slotId).FirstOrDefault();
            List<Connections> connections = new List<Connections>();
            if (isDport)
            {
                connections = _dbContext.Connections.Where(e => e.DestinationServerId == info.DServer).ToList();
            }
            else
            {
                connections = _dbContext.Connections.Where(e => e.SourceServerId == info.SServer).ToList();
            }
            if (slotRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + slotRec.ImagePath;
                var fileName = Path.GetFileName(ImagePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ImagePath);
                var ports = _dbContext.ServerPortDtl.Where(e => e.ServerId == slotRec.Id).ToList();
                if (ports.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        for (int i = 0; i < bmp.Width; i++)
                        {
                            for (int j = 0; j < bmp.Height; j++)
                            {
                                foreach (var p in ports)
                                {
                                    Pen pen = new Pen(Color.Red, 5);
                                    if (isDport)
                                    {
                                        var connected = connections.Where(e => e.DestinationPortId == p.Id).FirstOrDefault();
                                        if (connected != null)
                                        {
                                            pen = new Pen(Color.Blue, 5);
                                        }
                                    }
                                    else
                                    {
                                        var connected = connections.Where(e => e.SourcePortId == p.Id).FirstOrDefault();
                                        if (connected != null)
                                        {
                                            pen = new Pen(Color.Blue, 5);
                                        }
                                    }
                                    if (p.Id == portId)
                                    {
                                        pen = new Pen(Color.Yellow, 5);
                                    }

                                    Rectangle rect = new Rectangle(p.Xcoordinate, p.Ycoordinate, p.RectangleXcoordinate, p.RectangleYcoordinate);
                                    using (Graphics G = Graphics.FromImage(bmp))
                                    {
                                        G.DrawRectangle(pen, rect);
                                    }

                                    i++;
                                }
                            }

                        }
                        bmp.Save(@"C:\temp\" + fileName, ImageFormat.Png); // ImageFormat.Jpeg, etc
                    }
                    string path = @"c:\temp\" + fileName;
                    base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    System.IO.File.Delete(path);
                }
            }
            return base64String;
        }


        public string ServerPortView(DCIMModel info, bool isDport)
        {
            string base64String = string.Empty;
            int serveId = isDport ? info.DServer : info.SServer;
            int slotId = isDport ? info.DPort : info.SPort;
            var serverRec = _dbContext.Server.Where(e => e.Id == serveId).FirstOrDefault();
            if (serverRec != null)
            {
                string ImagePath = _hostingEnvironment.ContentRootPath + serverRec.ImagePath;
                var fileName = Path.GetFileName(ImagePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ImagePath);
                var slots = _dbContext.ServerPortDtl.Where(e => e.ServerId == serverRec.Id).ToList();
                if (slots.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        for (int i = 0; i < bmp.Width; i++)
                        {
                            for (int j = 0; j < bmp.Height; j++)
                            {
                                foreach (var p in slots)
                                {
                                    Pen pen = new Pen(Color.Red, 5);
                                    if (p.Id == slotId)
                                    {
                                        pen = new Pen(Color.Yellow, 5);
                                    }

                                    Rectangle rect = new Rectangle(p.Xcoordinate, p.Ycoordinate, p.RectangleXcoordinate, p.RectangleYcoordinate);
                                    using (Graphics G = Graphics.FromImage(bmp))
                                    {
                                        G.DrawRectangle(pen, rect);
                                    }

                                    i++;
                                }
                            }

                        }
                        bmp.Save(@"C:\temp\" + fileName, ImageFormat.Png); // ImageFormat.Jpeg, etc
                    }
                    string path = @"c:\temp\" + fileName;
                    base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                    System.IO.File.Delete(path);
                }
            }
            return base64String;
        }


        public List<PortMaster> GetPortMasters()
        {
            return _dbContext.PortMaster.ToList();
        }
        public string MakePortReplace(DCIMModel info)
        {
            string base64String = string.Empty;
            ServerPortDtl port = _dbContext.ServerPortDtl.Where(a => a.Id == info.SPort).FirstOrDefault();
            PortMaster portMaster = _dbContext.PortMaster.Where(e => e.Id == info.DPort).FirstOrDefault();
            Server server = _dbContext.Server.Where(e => e.Id == info.SServer).FirstOrDefault();
            Bitmap src = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + server.ImagePath) as Bitmap;
            Bitmap dest = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + portMaster.ImagePath) as Bitmap;
            var target = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.DrawImage(src, new Rectangle(new Point(), src.Size),
                    new Rectangle(new Point(), src.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(dest, new Rectangle(new Point(port.Xcoordinate, port.Ycoordinate), new Size(port.RectangleXcoordinate, port.RectangleYcoordinate)),
                                 new Rectangle(0, 0, dest.Width, dest.Height),
                                 GraphicsUnit.Pixel);
            }
            string filePath = server.ImagePath;
            src.Dispose();
            target.Save(_hostingEnvironment.ContentRootPath + filePath);
            dest.Dispose();


            base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(_hostingEnvironment.ContentRootPath + filePath));
            port.Port = portMaster.PortName;
            _dbContext.ServerPortDtl.Update(port);
            _dbContext.SaveChanges();
            return base64String;
        }
        public string GetReplacePortImage(DCIMModel info)
        {
            string base64String = string.Empty;
            string Image = _dbContext.PortMaster.Where(e => e.Id == info.DPort).Select(e => e.ImagePath).FirstOrDefault();
            string ImagePath = _hostingEnvironment.ContentRootPath + Image;
            base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(ImagePath));
            return base64String;
        }

        public string GetSlotImages(int SlotId)
        {
            string base64String = string.Empty;
            string Image = _dbContext.SlotMaster.Where(e => e.Id == SlotId)
                .Select(e => e.ImagePath).FirstOrDefault();
            string ImagePath = _hostingEnvironment.ContentRootPath + Image;
            base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(ImagePath));
            return base64String;
        }

        public List<SlotMaster> GetSlotMasters()
        {
            return _dbContext.SlotMaster.ToList();
        }

        public (int, string) InsertSlotDetails(DCIMModel dCIMModel)
        {
            var dSlot = _dbContext.SlotMaster
                .Where(a => a.Id == dCIMModel.DSlot).FirstOrDefault();
            var dSlotPorts = _dbContext.SlotPort
                .Where(a => a.SlotId == dCIMModel.DSlot).ToList();
            var sSlot = _dbContext.ServerSlotDtl
                .Where(a => a.Id == dCIMModel.SSlot).FirstOrDefault();
            var sPortDtl = _dbContext.ServerPortDtl
                .Where(a => a.ServerSlotDtlId == dCIMModel.SSlot).ToList();
            var Server = _dbContext.Server
                .Where(a => a.Id == dCIMModel.SServer).FirstOrDefault();
            //this needs to be revisited
            float maxWidth = dSlot.RectangleXcoordinate;
            float minWidth = sSlot.RectangleXcoordinate;
            float maxHeight = dSlot.RectangleYcoordinate;
            float minHeight = sSlot.RectangleYcoordinate;
            float widthDiv = maxWidth / minWidth;
            float heightDiv = maxHeight / minHeight;
            if (sPortDtl.Count > 0)
            {
                _dbContext.RemoveRange(sPortDtl);
                dSlotPorts.ForEach(a =>
                {
                    
                    ServerPortDtl ServerPortDtl = new ServerPortDtl();
                    ServerPortDtl.Xcoordinate = Convert.ToInt32(a.Xcoordinate/widthDiv)+sSlot.Xcoordinate;
                    ServerPortDtl.Ycoordinate = Convert.ToInt32(a.Ycoordinate / heightDiv)+sSlot.Ycoordinate;
                    ServerPortDtl.RectangleXcoordinate = Convert.ToInt32(a.RectangleXcoordinate.Value/widthDiv);
                    ServerPortDtl.RectangleYcoordinate = Convert.ToInt32(a.RectangleYcoordinate.Value/heightDiv);
                    ServerPortDtl.Port = a.PortName;
                    ServerPortDtl.ServerId = sSlot.ServerId.Value;
                    ServerPortDtl.ServerSlotDtlId = sSlot.Id;
                    ServerPortDtl.Description = "desc";
                    _dbContext.Add(ServerPortDtl);

                });
            }
            else
            {
                dSlotPorts.ForEach(a =>
                {
                    ServerPortDtl ServerPortDtl = new ServerPortDtl();
                    ServerPortDtl.Xcoordinate = Convert.ToInt32(a.Xcoordinate / widthDiv) + sSlot.Xcoordinate;
                    ServerPortDtl.Ycoordinate = Convert.ToInt32(a.Ycoordinate / heightDiv) + sSlot.Ycoordinate;
                    ServerPortDtl.RectangleXcoordinate = Convert.ToInt32(a.RectangleXcoordinate.Value / widthDiv);
                    ServerPortDtl.RectangleYcoordinate = Convert.ToInt32(a.RectangleYcoordinate.Value / heightDiv);
                    ServerPortDtl.Port = a.PortName;
                    ServerPortDtl.ServerId = sSlot.ServerId.Value;
                    ServerPortDtl.ServerSlotDtlId = sSlot.Id;
                    ServerPortDtl.Description = "desc";
                    _dbContext.Add(ServerPortDtl);

                });
            }
            Bitmap src1 = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + Server.ImagePath) as Bitmap;
            Bitmap dest1 = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + dSlot.ImagePath) as Bitmap;
            var target1 = new Bitmap(src1.Width, src1.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(target1))
            {
                graphics.DrawImage(src1, new Rectangle(new Point(), src1.Size),
                    new Rectangle(new Point(), src1.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(dest1, new Rectangle(new Point(sSlot.Xcoordinate, sSlot.Ycoordinate),
                    new Size(sSlot.RectangleXcoordinate, sSlot.RectangleYcoordinate)),
                                 new Rectangle(0, 0, dest1.Width, dest1.Height),
                                 GraphicsUnit.Pixel);
                graphics.Dispose();
                src1.Dispose();
            }
            string filePath1 = Server.ImagePath;
            int success = 0;
            success = _dbContext.SaveChanges();
            target1.Save(_hostingEnvironment.ContentRootPath + filePath1);
            dest1.Dispose();
            string base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(_hostingEnvironment.ContentRootPath + filePath1));

            return (success, base64String);
        }

        public string MapServerToRack(RackServerMap rackServerMap)
        {

            string base64String = string.Empty;
            var rack = _dbContext.Rack.Where(e => e.Id == rackServerMap.Rack).FirstOrDefault();
            var rackUnit = _dbContext.RackUnit.Where(e => e.Id == rackServerMap.RackUnit).FirstOrDefault();
            var server = _dbContext.Server.Where(e => e.Id == rackServerMap.Server).FirstOrDefault();
            if (server != null)
            {

                Bitmap src = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + rack.ImagePath) as Bitmap;
                Bitmap dest = System.Drawing.Image.FromFile(_hostingEnvironment.ContentRootPath + server.ImagePath) as Bitmap;
                var target = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.DrawImage(src, new Rectangle(new Point(), src.Size),
                        new Rectangle(new Point(), src.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(dest, new Rectangle(new Point(rackUnit.Xcoordinate, rackUnit.Ycoordinate), new Size(rackUnit.RectangleXcoordinate, rackUnit.RectangleYcoordinate)),
                                     new Rectangle(0, 0, dest.Width, dest.Height),
                                     GraphicsUnit.Pixel);
                }
                string filePath = rack.ImagePath;
                src.Dispose();
                target.Save(_hostingEnvironment.ContentRootPath + filePath);
                dest.Dispose();

                base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(_hostingEnvironment.ContentRootPath + filePath));

                server.RackId = rackServerMap.Rack;
                server.RackUnitId = rackServerMap.RackUnit;
                _dbContext.Update(server);
                _dbContext.SaveChanges();
            }

            return base64String;
        }
    }
}
