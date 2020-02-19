local nk = require("nakama")

local function is_empty(str)
    return str == nil or str == ""
end

local function create_server(context, payload)
    local json = nk.json_decode(payload)

    if not is_empty(json.server_name) and not is_empty(json.server_port) then
        local match = {
            { collection = "matches", key = context.client_ip, value = { server_name = json.server_name, server_ip = context.client_ip, server_port = json.server_port }}
        }

        nk.storage_write(match)
    end
end

local function get_servers(context, payload)
    return nk.json_encode(nk.storage_list("", "matches", 99, ""))
end

nk.register_rpc(create_server, "create_server")
nk.register_rpc(get_servers, "get_servers")
