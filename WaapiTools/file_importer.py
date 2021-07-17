import os
from waapi import WaapiClient, CannotConnectToWaapiException




def import_and_create_event(file_path):
    def get_file_imports(folderpath):

        file_paths=[]
        if(folderpath):
            files = os.listdir(folderpath)
            for i in files:
                if (not os.path.isdir(i))and(i.endswith('wav')):
                    file_path=folderpath+'/'+i
                    file_paths.append({"audioFile": file_path, "objectPath": "\\Actor-Mixer Hierarchy\\Default Work Unit\\<Sound>{}".format(i.replace(".wav", ""))})#里面写Sound或者Sound SFX竟然是一样的
            
        else:
            print("no folder selected")
            os.system('pause')
        
        return file_paths

    import_paths = get_file_imports(file_path)
    try:
        
        with WaapiClient() as client:#"ws://127.0.0.1:8080/waapi"
    
            args={

            "importOperation": "useExisting", 

            "default": {"importLanguage": "SFX"}, 

            "imports": import_paths
            }
            
            client.call("ak.wwise.core.audio.import", args)

            for data in import_paths:
                path = data["objectPath"]
                name = path.split(">")[1]
                query={
                    "parent":"\\Events\\Default Work Unit",
                    "type":"Event",
                    "name": name,
                    "onNameConflict": "merge",
                    "children":[
                        {
                            "name":"",
                            "type":"Action",
                            "@ActionType":1,
                            "@Target":"\\Actor-Mixer Hierarchy\\Default Work Unit\\{}".format(name)
                        }
                    ]
                }
                client.call("ak.wwise.core.object.create", query)

    except CannotConnectToWaapiException:
        print("Could not connect to Waapi: Is Wwise running and Wwise Authoring API enabled?")