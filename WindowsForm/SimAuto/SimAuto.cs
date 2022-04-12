using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CypsaLive.SimAuto
{
    public class TSimAuto
    {
        public string PWCaseDir { get; set; }
        public string PWCaseFile { get; set; }
        public TStore myStore { get; set; }
        public Boolean caseIsOpen { get; set; } // kate 1/29/19 this should be protected and not public... figur eout how to do that in CSharp.
        public string errMessage { get; set; } // kate 1/29/19

        public pwrworld.SimulatorAuto mySimAuto { get; set; }
        public string Display
        {
            get
            {
                return String.Format("SimAuto Connector");
            }
        }


        // Other functions we need
        // 1. Get generator parameters
        // 2. Get bus parameters
        // 3. Get relay parameters
        // 4. Open all relays from cyber and get PI
        // 5. Get CCTs of all relays

        public TSimAuto(string PWCaseDir_, string PWCaseFile_) // constructor
        {
            // kate 10/23/18 - test with captain 8 bus (corrected) input files
            // PWCaseDir = "C:\\Users\\kdaLab\\Dropbox\\sharedwithKatePuter\\work\\CyPSA_PlusPlus\\InputFiles\\8busdemofiles"; // captain
            //PWCaseDir = "C:\\Users\\abhijeet_ntpc\\Desktop\\DataCyPSA\\InputFiles\\8busdemofiles";  // tamupc
            PWCaseDir = PWCaseDir_;
            PWCaseFile = PWCaseFile_;
            //PWCaseFile = "caseUSETHISONE.pwb";
            errMessage = "";
            caseIsOpen = false;
            // initiate the object and open the case 
            mySimAuto = new pwrworld.SimulatorAuto(); // currently giving an error


        }

        public string OpenCase()
        {
            object[] output = (object[])mySimAuto.OpenCase(PWCaseDir + "\\" + PWCaseFile);
            // kate 1/8/19 this should also *set the current directory* here -- this may or may not happen correctly by defualt, so we should set it
            errMessage = errMessage + String.Format("{0}", output[0]);
            // kate 1/28/19 need to print message to log

            // kate 1/28/19 error checking is missing!!
            if (errMessage != "")
            {
                caseIsOpen = false;
            }
            else
            {
                caseIsOpen = true;
            }
            return "SimAuto.OpenCase Error:" + errMessage;
        }

        public string SaveAndCloseCase()
        {
            // Save and close
            //string scriptCommand_saveCase = "SaveCase(\"" + PWCaseFile + "_mapped.pwb\",PWB)";
            string scriptCommand_saveCase = "SaveCase(\"" + PWCaseFile + "\",PWB)";
            mySimAuto.RunScriptCommand(scriptCommand_saveCase);
            return errMessage;
        }

        public void SetupBranchData()
        {
            // Figure out how the lines connect the buses and store the connectivity graph

            if (caseIsOpen) // kate 1/29/19 do not repopen the case every time
            {
                string objtype = "Branch";
                dynamic all_branch = mySimAuto.ListOfDevicesAsVariantStrings(objtype, "");
                //object[] fieldarray = { "BusNum", "BusName", "BusAngle" };
                object[] res = (object[])all_branch[1];
                object[] fromBuses = (object[])res[0];
                object[] toBuses = (object[])res[1];
                object[] statuses = (object[])res[2];

                int count = 0;

                for (int i = 0; i < fromBuses.Length; i++)
                {
                    myStore.BranchItems.Add(new TBranch
                    {
                        fromBus = String.Format("From {0} ", fromBuses[i].ToString().TrimStart()), //Bus num
                        toBus = String.Format("To {0}", toBuses[i].ToString().TrimStart()),
                        connected = Convert.ToInt16(statuses[i]) == 1 ? true : false
                    });
                    count = count + 1;

                    //    }
                }
            }
        }

        public void SetupBreakerData() // kate 1/29/19 what is this function for?
        {
            if (caseIsOpen)
            {
                string objtype = "Branch";
                dynamic all_branches = mySimAuto.ListOfDevicesAsVariantStrings(objtype, "");
                object[] fieldarray = { "AllLabels", "BusNameFrom", "BusNameTo", "Status" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                object[] breaker = (object[])output[1];

                object[] bName = (object[])breaker[0];
                object[] bFrom = (object[])breaker[1];
                object[] bTo = (object[])breaker[2];
                object[] bStatus = (object[])breaker[3];

                for (int i = 0; i < bName.Length; i++)
                {
                    //if (bName[i].ToString() == "Breaker")
                    //{
                    myStore.BreakerItems.Add(new TBreaker
                    {
                        name = bName[i].ToString(),
                        fromBus = bFrom[i].ToString(), //Bus num
                        toBus = bTo[i].ToString(),
                        status = bStatus[i].ToString()
                    });

                    //}
                }
            }
        }
        // Command to breaker
        public void BreakerCommand(string command) // kate 1/29/19 what is this function for? what is command? saving the case (should be seperate function)?
        {
            if (caseIsOpen)
            {
                //string objtype = "Branch";
                //object[] fieldarray = { "Status" };

                //Abhijeet 24/1/19: This function is not working ...Can you please check this function
                //output = mySimAuto.ChangeParametersMultipleElement(objtype,fieldarray, command);
                mySimAuto.RunScriptCommand(command);
                mySimAuto.SaveCase("case_mod", "pwb", true);
            }
        }

        // Abhijeet 24/1/2019  Reload breaker status on modification
        public void ReLoadBreakerData(string pwCase) // kate 1/28/19 same question??
        {
            if (caseIsOpen)
            {
                string objtype = "Branch";
                dynamic all_branches = mySimAuto.ListOfDevicesAsVariantStrings(objtype, "");
                object[] fieldarray = { "AllLabels", "BusNameFrom", "BusNameTo", "Status" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                object[] breaker = (object[])output[1];

                object[] bName = (object[])breaker[0];
                object[] bFrom = (object[])breaker[1];
                object[] bTo = (object[])breaker[2];
                object[] bStatus = (object[])breaker[3];

                myStore.BreakerItems.Clear();

                for (int i = 0; i < bName.Length; i++)
                {
                    //if (bName[i].ToString() == "Breaker")
                    //{
                    myStore.BreakerItems.Add(new TBreaker
                    {
                        name = bName[i].ToString(),
                        fromBus = bFrom[i].ToString(), //Bus num
                        toBus = bTo[i].ToString(),
                        status = bStatus[i].ToString()
                    });
                }
            }
        }

        // Abhijeet 24/1/2019 : OpenAllCyber by IP
        public List<double> OpenAllCyber(List<string> ips) // kate 1/28/19 this function was missing all error checking
        {
            List<double> pi = new List<double>();

            if (caseIsOpen)
            {
                string[] paramlist = new string[2];
                paramlist[0] = "OverloadRank";

                //var modeChange = mySimAuto.RunScriptCommand("EnterMode(EDIT);");
                var simAutoBaseOutput = mySimAuto.RunScriptCommand("CTGSetAsReference;"); // kate 1/28/19 the logic below was never restoring the reference
                for (int k = 0; k < ips.Count; k++)
                {
                    // ************** OPEN RELAY BY IP ****************
                    // OpenAllFromCyber([CyberDevice "10.50.50.129"]);
                    string script = "OpenAllFromCyber([CyberDevice \"" + ips[k] + "\" ]);";
                    dynamic simAutoRelayIPOutput = mySimAuto.RunScriptCommand(script);

                    // ************** SOLVE POWER FLOW ****************
                    // Calculation of Performance Index
                    dynamic simAutoPFOutput = mySimAuto.RunScriptCommand("EnterMode(RUN);");
                    simAutoPFOutput = mySimAuto.RunScriptCommand("SolvePowerFlow;");
                    if (simAutoPFOutput[0] != "")
                    {
                        errMessage = errMessage + '\n' + ips[k] + "caused a BLACKOUT!";
                        pi.Add(5000);
                        return pi; // if power flow didnt' solve, it was a blackout
                    }

                    // "************** GET PERFORMANCE INDEX *********************
                    dynamic simAutoPIOutput = mySimAuto.GetParametersMultipleElement("PWCaseInformation", paramlist, "");
                    pi.Add(Convert.ToDouble(simAutoPIOutput[1][0][0]));


                    // "************** RESTORE REFERENCE STATE *********************
                    simAutoBaseOutput = mySimAuto.RunScriptCommand("CTGRestoreReference;"); // kate 1/28/19 this was missing
                }

                mySimAuto.SaveCase("case_mod", "pwb", true);
                return pi;
            }
            return pi;
        }

        // Abhijeet 14/6/2019 : OpenAllBreaker for the models whose cyber part not modeled in PowerWorld
        public List<double> OpenAllBreaker(List<string> ips)
        {
            List<double> pi = new List<double>();
            if (caseIsOpen)
            {
                string[] paramlist = new string[2];
                paramlist[0] = "OverloadRank";
                string filePath = PWCaseDir + "\\relayBreaker.txt";
                var modeChange = mySimAuto.RunScriptCommand("EnterMode(EDIT);");
                string fromBus = "";
                string toBus = "";
                string cktId = "";
                //var simAutoBaseOutput = mySimAuto.RunScriptCommand("CTGSetAsReference;"); // kate 1/28/19 the logic below was never restoring the reference
                for (int k = 0; k < ips.Count; k++)
                {
                    // ************** GET THE BREAKER INFO FROM THE IP****************
                    using (var reader = new StreamReader(filePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (line == null || line.Length == 0) continue;
                            if (line.Contains(ips[k]))
                            {
                                fromBus = line.Split(',')[1];
                                toBus = line.Split(',')[2];
                                cktId = line.Split(',')[3];
                                break;
                            }
                        }
                    }

                    string OpenBranch = "SetData(Branch,[BusNum, BusNum: 1, LineCircuit, LineStatus], [" + fromBus.ToString() + "," + toBus.ToString() + "," + cktId.ToString() + "," + "\"Open\"])";
                    var Open = mySimAuto.RunScriptCommand(OpenBranch);

                    // ************** SOLVE POWER FLOW ****************
                    // Calculation of Performance Index
                    dynamic simAutoPFOutput = mySimAuto.RunScriptCommand("EnterMode(RUN);");
                    simAutoPFOutput = mySimAuto.RunScriptCommand("SolvePowerFlow;");
                    if (simAutoPFOutput[0] != "")
                    {
                        errMessage = errMessage + '\n' + ips[k] + "caused a BLACKOUT!";
                        pi.Add(5000);
                        return pi; // if power flow didnt' solve, it was a blackout
                    }

                    // "************** GET PERFORMANCE INDEX *********************
                    dynamic simAutoPIOutput = mySimAuto.GetParametersMultipleElement("PWCaseInformation", paramlist, "");
                    pi.Add(Convert.ToDouble(simAutoPIOutput[1][0][0]));

                    // "************** RESTORE REFERENCE STATE *********************
                    string CloseBranch = "SetData(Branch,[BusNum, BusNum: 1, LineCircuit, LineStatus], [" + fromBus.ToString() + "," + toBus.ToString() + "," + cktId.ToString() + "," + "\"Closed\"])";
                    var Close = mySimAuto.RunScriptCommand(CloseBranch);

                }
                mySimAuto.SaveCase("case_mod", "pwb", true);
                return pi;
            }
            return pi;
        }

        // Abhijeet 24/1/2019 :Populate the list of contingencies
        public void SetContingencies()
        {
            if (caseIsOpen)
            {
                string objtype = "ContingencyElement";
                var res = mySimAuto.GetFieldList(objtype);
                object[] fieldarray = { "Contingency" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                object[] clist = (object[])output[1];
                object[] cname = (object[])clist[0];
                for (int i = 0; i < cname.Length; i++)
                {

                    // Add fields in an array
                    myStore.CtgItems.Add(new TContingency
                    {
                        Name = cname[i].ToString()
                    });

                }
            }

        }

        public Dictionary<int, List<double>> testbusdisplay_data(string displayFile)
        {
            // read the axd file for the display
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\abhijeet_ntpc\Desktop\display.axd");
            string[] lines = System.IO.File.ReadAllLines(displayFile);
            int numLines = 0, startLine = 0, endLine = 0;
            Dictionary<int, List<double>> data = new Dictionary<int, List<double>>();
            foreach (string line in lines)
            {
                numLines++;
                if (line[0] == '{') startLine = numLines;// start reading the content after this line 
                if (line[0] == '}') endLine = numLines;
                if (numLines > startLine && startLine != 0 && endLine == 0)
                {
                    var nline = Regex.Replace(line, " {1,}", ",");
                    var components = nline.Split(',');
                    int bus = Convert.ToInt32(components[1].Trim());
                    double xcord = Convert.ToDouble(components[3].Trim());
                    double ycord = Convert.ToDouble(components[4].Trim());
                    List<double> coordinate = new List<double>();
                    coordinate.Add(xcord); coordinate.Add(ycord);
                    data.Add(bus, coordinate);
                }

            }

            return data;
        }
        public void SetupBusData(bool subDefined, string displayFile)
        {
            if (caseIsOpen)
            {
                string objtype = "Bus";
                //object[] all_buses = mySimAuto.ListOfDevicesAsVariantStrings(objtype, ""); // not currently used
                object[] fieldarray = { "BusNum", "BusName", "BusAngle", "SubNum" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                object[] ng = (object[])output[1];
                object[] busno = (object[])ng[0];
                object[] busname = (object[])ng[1];
                object[] busang = (object[])ng[2];
                object[] subnum = (object[])ng[3];

                //string disObjtype = "DisplayBus";
                ////object[] all_buses = mySimAuto.ListOfDevicesAsVariantStrings(objtype, ""); // not currently used
                //object[] disFieldarray = { "BusNum", "SOX", "SOY" };
                ////object[] disFieldarray = { "BusNum", "SOX", "SOY", "SOSize", "SOWidth", "SOOrientation"};
                //object[] disOutput = mySimAuto.GetParametersMultipleElement(disObjtype, disFieldarray, "");
                //object[] disng = (object[])disOutput[1];
                //object[] disbusNum = (object[])disng[0];
                //object[] disbusX = (object[])disng[1];
                //object[] disbusY = (object[])disng[2];
                //int[] disBusnumArray = Array.ConvertAll<object, int>(disbusNum, (o) => (int)o);
                //double[] disBusXArray = Array.ConvertAll<object, double>(disbusX, (o) => (double)o);
                //double[] disBusYArray = Array.ConvertAll<object, double>(disbusY, (o) => (double)o);
                Dictionary<int, List<Double>> data = new Dictionary<int, List<double>>();
                if (subDefined)
                    data = testbusdisplay_data(displayFile);

                int numbuses = busno.Length;
                int count = 0;
                for (int i = 0; i < numbuses; i++)
                {

                    //int index = Array.IndexOf(disBusnumArray, (i + 1));
                    // kate 10/24/18 create a new vendor from busno[i] if it doesn't already exist    

                    // if substation not allocated
                    if (!subDefined) subnum[i] = busno[i];
                    if (subDefined)
                    {
                        myStore.BusItems.Add(new TBus
                        {
                            id = String.Format("Bus {0}", busno[i]), //Bus num
                            Name = String.Format("Bus {0}", busname[i]),
                            busAngle = Convert.ToDecimal(busang[i]),
                            subnum = Convert.ToInt32(subnum[i]), // for IEEE 24 case there is no substation info
                            xcord = data[i + 1][0],
                            ycord = data[i + 1][1]
                        });
                    }
                    else
                    {
                        myStore.BusItems.Add(new TBus
                        {
                            id = String.Format("Bus {0}", busno[i]), //Bus num
                            Name = String.Format("Bus {0}", busname[i]),
                            busAngle = Convert.ToDecimal(busang[i]),
                            subnum = Convert.ToInt32(subnum[i]) // for IEEE 24 case there is no substation info
                        });
                    }
                    count = count + 1;
                }
            }

        }

        public void SetupRelayData(bool cyberDefined)
        {
            if (cyberDefined)
            {
                if (caseIsOpen)
                {
                    string objtype = "CyberDevice";
                    dynamic all_relays = mySimAuto.ListOfDevicesAsVariantStrings(objtype, "");
                    object[] fieldarray = { "Name", "RelayType", "RelayAddress" };
                    dynamic output = mySimAuto.GetParametersMultipleElement("CyberDevice", fieldarray, "");

                    object[] relay = (object[])output[1];
                    object[] rName = (object[])relay[0];
                    object[] rType = (object[])relay[1];
                    object[] rIp = (object[])relay[2];

                    for (int i = 0; i < rName.Length; i++)
                    {
                        // Add fields in an array
                        myStore.RelayItems.Add(new TRelay
                        {
                            relayName = String.Format("{0} ", rName[i].ToString().Substring(0, 4)), //Bus num
                            relayType = String.Format("{0}", rType[i].ToString().Replace("snet:", "")),
                            relayIP = rIp[i].ToString()
                        });
                    }

                }
            }
            else
            {
                string filePath = PWCaseDir + "relayBreaker.txt";
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line == null || line.Length == 0) continue;
                        else
                        {
                            myStore.RelayItems.Add(new TRelay
                            {
                                relayName = "Branch_" + line.Split(',')[1] + "_" + line.Split(',')[2] + "_" + line.Split(',')[3], //Bus num
                                relayType = "Over Current Relay",
                                relayIP = line.Split(',')[0]
                            });
                        }
                    }
                }
            }

        }

        public void SetupGenData()
        {
            if (caseIsOpen)
            {
                ///////////////
                // Example of reading generator paramters
                string objtype = "Gen";
                //object[] all_gens = mySimAuto.ListOfDevicesAsVariantStrings(objtype, ""); // kate 1/28/19 this currently does nothing
                object[] fieldarray = { "GenMW", "BusNum", "GenID" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                errMessage = errMessage + String.Format("{0}", output[0]);
                if (errMessage != "")
                {
                    return;
                }
                // kate 10/23/18 put these into the generator objects
                object[] ng = (object[])output[1];
                object[] mw = (object[])ng[0];
                object[] busno = (object[])ng[1];
                object[] gid = (object[])ng[2];

                int numgens = mw.Length;
                //string govname = gov[1];

                // kate 10/23/18 do something like the below to import data for all of the relays, and then change them
                int count = 0;
                for (int i = 0; i < numgens; i++)
                {

                    // kate 10/24/18 create a new vendor from busno[i] if it doesn't already exist              
                    // Add fields in an array
                    myStore.GenItems.Add(new TGen
                    {
                        id = String.Format("Gen {0} {1}", busno[i].ToString().TrimStart(), gid[i]), //Gen ID
                        Name = String.Format("{0}", gid[i]), // "A book about a whale",
                        mw = Convert.ToDecimal(mw[i]), // The "M" after 4.50 indicates it is a decimal and not a double
                    });
                    count = count + 1;

                }

            }
        }

        public void SetupLoadData()
        {
            if (caseIsOpen)
            {
                ///////////////
                // Example of reading generator paramters
                string objtype = "Load";
                //object[] all_gens = mySimAuto.ListOfDevicesAsVariantStrings(objtype, ""); // kate 1/28/19 this currently does nothing
                object[] fieldarray = { "BusNum", "LoadID", "SubName" };
                dynamic output = mySimAuto.GetParametersMultipleElement(objtype, fieldarray, "");
                errMessage = errMessage + String.Format("{0}", output[0]);
                if (errMessage != "")
                {
                    return;
                }

                object[] nl = (object[])output[1];
                object[] busno = (object[])nl[0];
                object[] lid = (object[])nl[1];
                object[] subname = (object[])nl[2];

                int numloads = busno.Length;
                //string govname = gov[1];

                // kate 10/23/18 do something like the below to import data for all of the relays, and then change them
                int count = 0;
                for (int i = 0; i < numloads; i++)
                {

                    // kate 10/24/18 create a new vendor from busno[i] if it doesn't already exist              
                    // Add fields in an array
                    myStore.LoadItems.Add(new TLoad
                    {
                        id = String.Format("Load {0} {1}", busno[i].ToString().TrimStart(), lid[i]), //Load ID
                        Name = String.Format("{0}", lid[i]),
                        subname = String.Format("{0}", subname[i]) // Substation Name

                    });
                    count = count + 1;

                }

            }
        }
    }


}
